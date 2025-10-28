using Holtz.SemanticKernel.Shared.Settings;
using Langfuse.OpenTelemetry;
using OpenTelemetry;
using OpenTelemetry.Trace;

namespace Holtz.SemanticKernel.Shared.OpenTelemetry
{
    /// <summary>
    /// https://github.com/carllapierre/langfuse-otel-dotnet
    /// </summary>
    public static class NugetPackageLangfuseOpenTelemetry
    {
        public static TracerProvider SetupTracerProvider()
        {
            // Enable GenAI diagnostics (prompts, tokens, completions)
            AppContext.SetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnosticsSensitive", true);

            return Sdk.CreateTracerProviderBuilder()
                                    //.AddSource("Microsoft.SemanticKernel*")
                                    .AddSource("*")
                                    .AddSource("Microsoft.Extensions.AI.OpenTelemetryChatClient")
                                    .AddSource("Microsoft.Extensions.AI")
                                    .AddSource("Microsoft.SemanticKernel")
                .AddLangfuseExporter(options =>
                {
                    options.PublicKey = SettingsHelper.Settings.Langfuse.PublicKey;
                    options.SecretKey = SettingsHelper.Settings.Langfuse.PrivateKey;
                    options.BaseUrl = SettingsHelper.Settings.Langfuse.BaseUrl;
                })
                .Build();
        }
    }
}
