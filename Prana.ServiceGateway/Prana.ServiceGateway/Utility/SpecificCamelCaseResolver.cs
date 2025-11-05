using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using System.Text.Json;

namespace Prana.ServiceGateway.Utility
{
    /// <summary>  
    /// A custom contract resolver for Newtonsoft.Json that applies camelCase naming  
    /// only to properties of a specific target class during serialization.  
    /// For other classes, the default naming strategy is used.  
    /// </summary> 
    public class SpecificCamelCaseResolver : DefaultContractResolver
    {
        private readonly System.Type _targetType;

        public SpecificCamelCaseResolver(System.Type targetType)
        {
            _targetType = targetType;
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            // Apply camelCase only if serializing the specific class
            return base.ResolvePropertyName(propertyName);
        }

        protected override JsonContract CreateContract(System.Type objectType)
        {
            if (objectType == _targetType)
            {
                this.NamingStrategy = new CamelCaseNamingStrategy();
            }
            else
            {
                this.NamingStrategy = new DefaultNamingStrategy();
            }
            return base.CreateContract(objectType);
        }

        public static JsonSerializerSettings GetSettingsForType<T>()
        {
            return GetSettingsForType(typeof(T));
        }

        private static JsonSerializerSettings GetSettingsForType(Type type)
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new SpecificCamelCaseResolver(type)
            };
        }
    }


    public class RequestBodyCaptureMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestBodyCaptureMiddleware> _logger;

        public RequestBodyCaptureMiddleware(RequestDelegate next, ILogger<RequestBodyCaptureMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            using var reader = new StreamReader(
                context.Request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);

            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            if (!string.IsNullOrWhiteSpace(body) && body !="null")
            {
                // Optionally: Store in Items for later use in controllers or filters
                context.Items["RawRequestBody"] = body;

                try
                {
                    var incomingDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(body);
                    var routeEndpoint = context.GetEndpoint() as RouteEndpoint;

                    if (routeEndpoint != null)
                    {
                        var metadata = routeEndpoint.Metadata
                            .OfType<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>()
                            .FirstOrDefault();

                        if (metadata != null)
                        {
                            var dtoType = metadata.Parameters
                                .FirstOrDefault(p => p.ParameterType.Name.EndsWith("RequestDto"))?.ParameterType;

                            if (dtoType != null)
                            {
                                var dtoProperties = dtoType
                                    .GetProperties()
                                    .Select(p => p.Name)
                                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                                var missingKeys = incomingDict.Keys
                                    .Where(key => !dtoProperties.Contains(key))
                                    .ToList();

                                if (missingKeys.Any())
                                {
                                    _logger.LogWarning("Incoming JSON has properties not present in DTO {DtoType}: {MissingKeys}",
                                        dtoType.Name,
                                        string.Join(", ", missingKeys));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error comparing incoming JSON to DTO properties.");
                }
            }

            await _next(context);
        }
    }


}