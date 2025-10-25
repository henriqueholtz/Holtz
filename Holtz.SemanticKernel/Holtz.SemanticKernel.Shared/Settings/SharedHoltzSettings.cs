namespace Holtz.SemanticKernel.Shared.Settings
{
    public class SharedHoltzSettings
    {
        public SharedOllamaSettings Ollama { get; set; }
        public LangfuseSettings Langfuse{ get; set; }
    }

    public abstract class SharedLlmSettings
    {
        public double Temperature { get; set; }
        public string ModelId { get; set; }
    }

    public class SharedOllamaSettings : SharedLlmSettings
    {
        public string Endpoint { get; set; }
    }

    public class LangfuseSettings
    {
        /// <summary>
        /// <public_key>:<private_key>
        /// </summary>
        public string Auth { get { return $"{PublicKey}:{PrivateKey}"; } }

        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string BaseUrl { get; set; } = "http://localhost:3000";
    }
}
