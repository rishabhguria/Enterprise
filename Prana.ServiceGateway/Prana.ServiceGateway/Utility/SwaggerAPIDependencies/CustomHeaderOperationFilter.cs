using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Prana.ServiceGateway.Constants;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using Prana.ServiceGateway.ExceptionHandling;

namespace Prana.ServiceGateway.Utility.SwaggerAPIDependencies
{
    public class CustomHeaderOperationFilter : IOperationFilter
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<CustomHeaderOperationFilter> _logger;

        public CustomHeaderOperationFilter(IConfiguration configuration,
            ILogger<CustomHeaderOperationFilter> logger)
        {
            _configuration = configuration;
            this._logger = logger;
        }

        /// <summary>
        /// This Method handles the password encryption for login user endpoint for Swagger API Validation
        /// </summary>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            try
            {
                if (context.ApiDescription.RelativePath.Contains("/loginuser", StringComparison.OrdinalIgnoreCase))
                {
                    operation.Parameters ??= new List<OpenApiParameter>();

                    var swaggerClientHeader = new OpenApiParameter
                    {
                        Name = GlobalConstants.SWAGGER_CLIENT,
                        In = ParameterLocation.Header,
                        Description = "Indicates request made from Swagger . Do Not Edit",
                        Required = false,
                        Schema = new OpenApiSchema { Type = "string", Default = new OpenApiString("true"), ReadOnly = true }
                    };

                    operation.Parameters.Add(swaggerClientHeader);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Apply of CustomHeaderOperationFilter");
                throw;
            }
        }
    }
}
