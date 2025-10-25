using Microsoft.Extensions.Configuration;

namespace Holtz.SemanticKernel.Shared.Settings
{
    public static class SettingsHelper
    {
        public static SharedHoltzSettings Settings { get; set; }

        public static void LoadSettings()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) // Directory.GetCurrentDirectory()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            Settings = config.Get<SharedHoltzSettings>();
        }
    }
}
