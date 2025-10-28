using Holtz.SemanticKernel.Shared.Kernels;
using Microsoft.SemanticKernel;
using OpenTelemetry.Trace;

(Kernel kernel, TracerProvider tracerProvider) = Ollama.CreateSemanticKernelWithLangfuseAndThirdPartyOtlp();
//(Kernel kernel, TracerProvider tracerProvider) = OpenAIKernel.CreateSemanticKernelWithLangfuseAndThirdPartyOtlp();

string prompt1 = "What color is the sky?";
var result1 = await kernel.InvokePromptAsync(prompt1);
Console.WriteLine(result1);


Console.WriteLine();
//KernelArguments arguments = new() { { "topic", "sea" } };
//await foreach (var update in kernel.InvokePromptStreamingAsync("What color is the {{$topic}}? Provide a detailed explanation.", arguments))
//{
//    Console.Write(update);
//}

tracerProvider.ForceFlush(); // That's not necessary

Console.WriteLine();
Console.WriteLine("Finished");