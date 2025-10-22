// See https://aka.ms/new-console-template for more information
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Ollama;

#region Parameters

string modelId = "llama3.2";
float temperature = 0.5f;
Uri ollamaEndpoint = new Uri("http://localhost:11434");

#endregion

// https://github.com/microsoft/semantic-kernel/blob/main/dotnet/samples/GettingStarted/Step1_Create_Kernel.cs

Kernel kernel = Kernel.CreateBuilder()
            .AddOllamaChatCompletion(modelId, ollamaEndpoint)
            .Build();

// Example 1. Invoke the kernel with a prompt and display the result
Console.WriteLine(await kernel.InvokePromptAsync("What color is the sky?"));
Console.WriteLine();

// Example 2. Invoke the kernel with a templated prompt and display the result
KernelArguments arguments = new() { { "topic", "sea" } };
Console.WriteLine(await kernel.InvokePromptAsync("What color is the {{$topic}}?", arguments));
Console.WriteLine();

// Example 3. Invoke the kernel with a templated prompt and stream the results to the display
await foreach (var update in kernel.InvokePromptStreamingAsync("What color is the {{$topic}}? Provide a detailed explanation.", arguments))
{
    Console.Write(update);
}

Console.WriteLine(string.Empty);

// Example 4. Invoke the kernel with a templated prompt and execution settings
arguments = new(new OllamaPromptExecutionSettings { Temperature = temperature, ModelId = modelId }) { { "topic", "dogs" } };
Console.WriteLine(await kernel.InvokePromptAsync("Tell me a story about {{$topic}}", arguments));
