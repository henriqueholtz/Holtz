using Holtz.SemanticKernel.Shared.OpenTelemetry;
using Holtz.SemanticKernel.Shared.Settings;
using Microsoft.SemanticKernel;
using OpenTelemetry.Trace;

namespace Holtz.SemanticKernel.Shared.Kernels
{
    public static class Ollama
    {
        /// <summary>
        /// Semantic Kernel with data Langfuse via custom Open Telemetry
        /// </summary>
        /// <returns></returns>
        public static Kernel CreateSemanticKernelWithLangfuseAndOtlpHttpClient()
        {
            SettingsHelper.LoadSettings();
            OtlpHttpClient.ConfigureFromEnv();
            return Kernel.CreateBuilder()
                .AddOllamaChatCompletion(
                    SettingsHelper.Settings.Ollama.ModelId,
                    new Uri(SettingsHelper.Settings.Ollama.Endpoint)
                )
                .Build();
        }

        /// <summary>
        /// Semantic Kernel with data Langfuse via Open Telemetry from nuget package "Langfuse.OpenTelemetry"
        /// </summary>
        /// <returns></returns>
        public static (Kernel, TracerProvider) CreateSemanticKernelWithLangfuseAndThirdPartyOtlp()
        {
            SettingsHelper.LoadSettings();
            TracerProvider tracerProvider = NugetPackageLangfuseOpenTelemetry.SetupTracerProvider();

            Kernel kernel = Kernel.CreateBuilder()
                .AddOllamaChatCompletion(
                    SettingsHelper.Settings.Ollama.ModelId,
                    new Uri(SettingsHelper.Settings.Ollama.Endpoint)
                )
                .Build();

            return (kernel, tracerProvider);
        }
    }
}
