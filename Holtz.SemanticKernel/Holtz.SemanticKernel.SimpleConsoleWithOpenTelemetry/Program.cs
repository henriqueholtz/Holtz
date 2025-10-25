using Holtz.SemanticKernel.Shared.Kernels;
using Microsoft.SemanticKernel;
using OpenTelemetry.Trace;

(Kernel kernel, TracerProvider tracerProvider) = Ollama.CreateSemanticKernelWithLangfuseAndThirdPartyOtlp();

string prompt1 = "What color is the sky?";
var result1 = await kernel.InvokePromptAsync(prompt1);
Console.WriteLine(result1);

tracerProvider.ForceFlush(); // That's not necessary

Console.WriteLine();
Console.WriteLine("Finished");