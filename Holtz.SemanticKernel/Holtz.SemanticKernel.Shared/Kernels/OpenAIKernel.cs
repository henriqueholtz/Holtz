using Holtz.SemanticKernel.Shared.OpenTelemetry;
using Holtz.SemanticKernel.Shared.Settings;
using Microsoft.SemanticKernel;
using OpenTelemetry.Trace;

namespace Holtz.SemanticKernel.Shared.Kernels
{
    public static class OpenAIKernel
    {
        /// <summary>
        /// Semantic Kernel with data Langfuse via custom Open Telemetry
        /// </summary>
        /// <returns></returns>
        public static (Kernel, TracerProvider) CreateSemanticKernelWithLangfuseAndThirdPartyOtlp()
        {
            SettingsHelper.LoadSettings();
            TracerProvider tracerProvider = NugetPackageLangfuseOpenTelemetry.SetupTracerProvider();

            Kernel kernel = Kernel.CreateBuilder()

                .AddOpenAIChatCompletion(
                    SettingsHelper.Settings.OpenAI.ModelId,
                    SettingsHelper.Settings.OpenAI.ApiKey
                )
                .Build();

            return (kernel, tracerProvider);
        }
    }
}
