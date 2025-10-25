using Holtz.SemanticKernel.Shared.OpenTelemetry;
using Holtz.SemanticKernel.Shared.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using System;
using System.IO;

namespace Holtz.SemanticKernel.Shared.Kernels
{
    public static class Ollama
    {
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
    }
}
