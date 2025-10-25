using Holtz.SemanticKernel.Shared.Settings;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Holtz.SemanticKernel.Shared.OpenTelemetry
{

    /// <summary>
    /// Minimal OTLP/HTTP JSON sender (no external dependencies).
    /// Configuration via environment variables:
    /// LANGFUSE_OTLP_ENDPOINT - full endpoint URL (default: http://localhost:3000/api/public/otel/v1/traces)
    /// LANGFUSE_AUTH - either "pk-...:sk-..." (will be base64-encoded) or already base64-encoded string
    /// Simple OTLP/JSON client
    /// </summary>
    public static class OtlpHttpClient
    {
        private static readonly HttpClient _http = new HttpClient();

        // https://api.reference.langfuse.com/
        // https://api.reference.langfuse.com/#tag/opentelemetry/post/api/public/otel/v1/traces
        // Helper to send a single-span trace representing a Semantic Kernel prompt invocation
        public static async Task SendKernelInvocationTraceAsync(string traceName, string prompt, string output)
        {
            // Build OTLP JSON payload following OTLP/JSON rules (traceId/spanId hex, camelCase fields)
            string traceId = NewTraceIdHex();
            string spanId = NewSpanIdHex();
            string sessionId = NewSpanIdHex();
            long startNs = UnixNanoNow();

            // Minimal span
            var span = new
            {
                traceId,
                spanId,
                name = traceName,
                kind = 1, // 1 = INTERNAL
                startTimeUnixNano = startNs.ToString(),
                endTimeUnixNano = UnixNanoNow().ToString(),
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
                var resp = await SendTracesJsonAsync(payload).ConfigureAwait(false);
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


        public static void ConfigureFromEnv()
        {
            var auth = SettingsHelper.Settings.Langfuse.Auth; 
            if (!string.IsNullOrEmpty(auth))
            {
                string headerValue = auth.Contains(":") ? Convert.ToBase64String(Encoding.UTF8.GetBytes(auth)) : auth;
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", headerValue);
            }
            // keep-alive is default; other headers can be added here
        }

        public static async Task<HttpResponseMessage> SendTracesJsonAsync(object payload)
        {
            var endpoint = $"{SettingsHelper.Settings.Langfuse.BaseUrl}/api/public/otel/v1/traces";
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
}
