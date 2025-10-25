using Holtz.SemanticKernel.Shared.Kernels;
using Holtz.SemanticKernel.Shared.OpenTelemetry;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Ollama;

public static class Program
{
    public static async Task Main(string[] args)
    {
        // Create kernel
        Kernel kernel = Ollama.CreateSemanticKernelWithLangfuseAndOtlpHttpClient();

        // Example 1. Invoke the kernel with a prompt and display the result, then send a trace
        string prompt1 = "What color is the sky?";
        var result1 = await kernel.InvokePromptAsync(prompt1);
        Console.WriteLine(result1);
        await OtlpHttpClient.SendKernelInvocationTraceAsync("kernel.invoke", prompt1, result1.ToString());
        Console.WriteLine();

        // Example 2. Invoke the kernel with a templated prompt and display the result
        KernelArguments arguments = new() { { "topic", "sea" } };
        string prompt2 = "What color is the {{$topic}}?";
        var result2 = await kernel.InvokePromptAsync(prompt2, arguments);
        Console.WriteLine(result2);
        await OtlpHttpClient.SendKernelInvocationTraceAsync("kernel.invoke", prompt2, result2.ToString());
        Console.WriteLine();

        // Example 3. Invoke the kernel with a templated prompt and stream the results to the display
        //await foreach (var update in kernel.InvokePromptStreamingAsync("What color is the {{$topic}}? Provide a detailed explanation.", arguments))
        //{
        //    Console.Write(update);
        //}

        Console.WriteLine(string.Empty);

        // Example 4. Invoke the kernel with a templated prompt and execution settings
        arguments = new(new OllamaPromptExecutionSettings { Temperature = 05 }) { { "topic", "dogs" } };
        string prompt4 = "Tell me a story about {{$topic}}";
        var result4 = await kernel.InvokePromptAsync(prompt4, arguments);
        Console.WriteLine(result4);
        await OtlpHttpClient.SendKernelInvocationTraceAsync("kernel.invoke", prompt4, result4.ToString());

        Console.WriteLine();
        Console.WriteLine("Finished!");
    }
}


