using Microsoft.AspNetCore.Mvc;

namespace Holtz.Versioning.Api
{
    public class ApiVersioning
    {
        public static ApiVersion DefaultApiVersion { get;  } = new ApiVersion(2, 0);
        public static string ApiVersionAsString { get; } = "x-api-version";
    }
}
