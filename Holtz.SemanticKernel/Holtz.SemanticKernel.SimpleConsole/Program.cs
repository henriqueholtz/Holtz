// See https://aka.ms/new-console-template for more information
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Ollama;

// Minimal OTLP/HTTP JSON sender (no external dependencies).
// Configuration via environment variables:
// LANGFUSE_OTLP_ENDPOINT - full endpoint URL (default: http://localhost:3000/api/public/otel/v1/traces)
// LANGFUSE_AUTH - either "pk-...:sk-..." (will be base64-encoded) or already base64-encoded string

// Simple OTLP/JSON client
static class OtlpHttpClient
{
    private static readonly HttpClient _http = new HttpClient();

    public static void ConfigureFromEnv()
    {
        var auth = Environment.GetEnvironmentVariable("LANGFUSE_AUTH") ?? ""; // <public_key>:<private_key>
        if (!string.IsNullOrEmpty(auth))
        {
            string headerValue = auth.Contains(":") ? Convert.ToBase64String(Encoding.UTF8.GetBytes(auth)) : auth;
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", headerValue);
        }
        // keep-alive is default; other headers can be added here
    }

    public static async Task<HttpResponseMessage> SendTracesJsonAsync(object payload)
    {
        var endpoint = Environment.GetEnvironmentVariable("LANGFUSE_OTLP_ENDPOINT") ?? "http://localhost:3000/api/public/otel/v1/traces";
        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        // OTLP servers expect application/json for OTLP/JSON
        return await _http.PostAsync(endpoint, content).ConfigureAwait(false);
    }

    public static string NewTraceIdHex()
    {
        // Guid is 16 bytes (128-bit) -> 32 hex chars
        return Guid.NewGuid().ToString("N");
    }

    public static string NewSpanIdHex()
    {
        Span<byte> b = stackalloc byte[8];
        RandomNumberGenerator.Fill(b);
        var sb = new StringBuilder(16);
        foreach (var x in b)
        {
            sb.Append(x.ToString("x2"));
        }
        return sb.ToString();
    }

    public static long UnixNanoNow()
    {
        var epoch = DateTime.UnixEpoch;
        var now = DateTime.UtcNow;
        // Ticks are 100ns. Multiply ticks by 100 to get ns
        return (now - epoch).Ticks * 100L;
    }
}

public static class Program
{
    public static async Task Main(string[] args)
    {
        #region Parameters

        string modelId = "llama3.2";
        float temperature = 0.5f;
        Uri ollamaEndpoint = new Uri("http://localhost:11434");

        #endregion

        // Create kernel
        Kernel kernel = Kernel.CreateBuilder()
                    .AddOllamaChatCompletion(modelId, ollamaEndpoint)
                    .Build();

        // Prepare HTTP client
        OtlpHttpClient.ConfigureFromEnv();

        // https://api.reference.langfuse.com/
        // https://api.reference.langfuse.com/#tag/opentelemetry/post/api/public/otel/v1/traces
        // Helper to send a single-span trace representing a Semantic Kernel prompt invocation
        async Task SendKernelInvocationTraceAsync(string traceName, string prompt, string output)
        {
            // Build OTLP JSON payload following OTLP/JSON rules (traceId/spanId hex, camelCase fields)
            string traceId = OtlpHttpClient.NewTraceIdHex();
            string spanId = OtlpHttpClient.NewSpanIdHex();
            string sessionId = OtlpHttpClient.NewSpanIdHex();
            long startNs = OtlpHttpClient.UnixNanoNow();

            // Minimal span
            var span = new
            {
                traceId = traceId,
                spanId = spanId,
                name = traceName,
                kind = 1, // 1 = INTERNAL
                startTimeUnixNano = startNs.ToString(),
                endTimeUnixNano = (OtlpHttpClient.UnixNanoNow()).ToString(),
                attributes = new[]
                {
                    new { key = "langfuse.observation.input", value = new { stringValue = prompt ?? string.Empty } },
                    new { key = "langfuse.observation.output", value = new { stringValue = output ?? string.Empty } },
                    new { key = "langfuse.observation.type", value = new { stringValue = "generation" } },
                },
                status = new { code = 0 }
            };

            var resourceSpan = new
            {
                resource = new { attributes = new[] { new { key = "service.name", value = new { stringValue = "Holtz.SemanticKernel.SimpleConsole" } } } },
                scopeSpans = new[]
                {
                    new {
                        scope = new { name = "manual", version = "1" },
                        spans = new[] { span }
                    }
                }
            };

            var payload = new { resourceSpans = new[] { resourceSpan } };

            try
            {
                var resp = await OtlpHttpClient.SendTracesJsonAsync(payload).ConfigureAwait(false);
                if (!resp.IsSuccessStatusCode)
                {
                    Console.Error.WriteLine($"OTLP send failed: {(int)resp.StatusCode} {resp.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"OTLP send exception: {ex.Message}");
            }
        }

        // Example 1. Invoke the kernel with a prompt and display the result, then send a trace
        string prompt1 = "What color is the sky?";
        var result1 = await kernel.InvokePromptAsync(prompt1);
        Console.WriteLine(result1);
        await SendKernelInvocationTraceAsync("kernel.invoke", prompt1, result1.ToString());
        Console.WriteLine();

        // Example 2. Invoke the kernel with a templated prompt and display the result
        KernelArguments arguments = new() { { "topic", "sea" } };
        string prompt2 = "What color is the {{$topic}}?";
        var result2 = await kernel.InvokePromptAsync(prompt2, arguments);
        Console.WriteLine(result2);
        await SendKernelInvocationTraceAsync("kernel.invoke", prompt2, result2.ToString());
        Console.WriteLine();

        // Example 3. Invoke the kernel with a templated prompt and stream the results to the display
        await foreach (var update in kernel.InvokePromptStreamingAsync("What color is the {{$topic}}? Provide a detailed explanation.", arguments))
        {
            Console.Write(update);
        }

        Console.WriteLine(string.Empty);

        // Example 4. Invoke the kernel with a templated prompt and execution settings
        arguments = new(new OllamaPromptExecutionSettings { Temperature = temperature, ModelId = modelId }) { { "topic", "dogs" } };
        string prompt4 = "Tell me a story about {{$topic}}";
        var result4 = await kernel.InvokePromptAsync(prompt4, arguments);
        Console.WriteLine(result4);
        await SendKernelInvocationTraceAsync("kernel.invoke", prompt4, result4.ToString());
    }
}


