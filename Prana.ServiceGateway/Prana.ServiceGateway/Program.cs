using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Contracts;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Hubs;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.Services;
using Prana.ServiceGateway.Utility;
using Prana.ServiceGateway.Utility.CustomMiddleware;
using Prana.ServiceGateway.Utility.SwaggerAPIDependencies;
using Serilog;
using Prana.ServiceGateway.ExceptionHandling;
using System.Reflection;

using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.CacheStore;
using Serilog.Debugging;
using Serilog.Events;
using SerilogTimings.Extensions;
using SerilogTimings;
using Microsoft.AspNetCore.Mvc;
using static Prana.ServiceGateway.Constants.GlobalConstants;
//using System.Threading.RateLimiting;
//using Microsoft.AspNetCore.RateLimiting;


namespace ServiceGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");

                Console.WriteLine("Starting service gateway for Environment: {0} ", builder.Environment.EnvironmentName);

                builder.Services.Configure<RateLimitingOptions>(builder.Configuration.GetSection("RateLimiting"));
                RateLimitingOptions rateLimitingConfig = builder.Configuration.GetSection("RateLimiting").Get<RateLimitingOptions>();

                //builder.Services.AddRateLimiter(options =>
                //{
                //    options.AddPolicy(rateLimitingConfig.PolicyName, context => CreateRateLimitPartition(context, rateLimitingConfig));

                //    options.OnRejected = (context, token) => HandleRateLimitRejection(context, rateLimitingConfig.RejectionMessage, token);
                //});

                builder.Services.AddSignalR();
                builder.Services.AddHealthChecks();

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration)
                    .Enrich.WithEnvironmentUserName()
                    .CreateLogger();


                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("CORSPolicy",
                        builder => builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowed((hosts) => true));
                });


                // Add services to the container.
                builder.Services.AddControllers();

                AddSingletonServices(builder);
                Log.Logger.Information("Added singleton, transient service in dependency container");

                builder.Services.AddDistributedMemoryCache();

                SwaggerConfiguration(builder);

                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = HelperFunctions.GetTokenValitionParam(builder.Configuration);
                });

                var jwtSection = builder.Configuration.GetSection("AuthSettings");
                var jwtOptions = new JwtOptions();
                jwtSection.Bind(jwtOptions);
                builder.Services.Configure<JwtOptions>(jwtSection);

                //Configuring the Serilog Logging Provider
                builder.Services.AddScoped<ILoggingHelper, LoggingHelper>();

                //builder.Logging.AddSerilog(Logger.SetupFileLogger(builder));
                //builder.Logging.AddSerilog(Logger.SetupConsoleLogger(builder));

                builder.Host.UseSerilog();

                var app = builder.Build();
                InstantiateServices(app);

                PermissionManager.GetInstance().Initialize(app.Services.GetService<IKafkaManager>());
                TradingTicketCache.GetInstance().Initialize(app.Services.GetService<IKafkaManager>());

                //Used for catching the close event for the API
                AppDomain.CurrentDomain.ProcessExit += (Sender, Event) =>
                {
                    ProcessExitHandler(Sender, Event, app);
                };

                //Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));

                var isSwaggerEnabled = builder.Configuration.GetValue<string>("EnvironmentVariables:IsSwaggerEnabled");

                Console.WriteLine("Is Swagger enabled " + isSwaggerEnabled);
                Console.WriteLine();
                if (Convert.ToBoolean(isSwaggerEnabled) || true)
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                app.Use(BlockMetaDataRequest(app.Configuration));

                app.UseCors("CORSPolicy");
                app.UseStatusCodePages();
                app.UseRouting();

                app.UseMiddleware<CustomRateLimitingMiddleware>();
                //app.UseRateLimiter();
                app.UseMiddleware<GlobalExceptionMiddleware>();

                app.UseMiddleware<TokenDecrypterMiddleware>();

                app.UseAuthentication();
                app.UseAuthorization();

                app.UseMiddleware<TokenManagerMiddleware>();

                app.UseMiddleware<LogEnricherMiddleware>();

                app.UseSerilogRequestLogging(options =>
                {
                    options.GetLevel = (httpContext, elapsed, ex) =>
                    {
                        //exlude url which we dont want to logs (frequent url like hearbeat etc)
                        //this should come from config
                        var path = httpContext.Request.Path;
                        if (path.StartsWithSegments("/serviceGatewayHub") ||
                            path.StartsWithSegments("/serviceGatewayHubRTPNL") ||
                            path.StartsWithSegments("/serviceGatewayHubRTPNLUpdates") ||
                            path.StartsWithSegments("/Logging/PostLogs") ||
                            path.StartsWithSegments("/Trading/SearchSymbol") ||
                            path.StartsWithSegments("/Trading/ValidateSymbol") ||
                            path.StartsWithSegments("/Trading/UnSubscribeSymbolCompressionFeed") ||
                            path.StartsWithSegments("/LiveFeed/UnSubscribeLiveFeed") ||
                            path.StartsWithSegments("/AuthenticateUser/GetConnectionStatus"))
                            return LogEventLevel.Verbose;

                        if (httpContext.Request.Method == "OPTIONS")
                            return LogEventLevel.Verbose;

                        return LogEventLevel.Information;
                    };
                });

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHub<ServiceGatewayHub>("/serviceGatewayHub");
                    endpoints.MapHub<ServiceGatewayHubRTPNL>("/serviceGatewayHubRTPNL");
                    endpoints.MapHub<ServiceGatewayHubRTPNLUpdates>("/serviceGatewayHubRTPNLUpdates");
                    endpoints.MapHealthChecks("/LoginUser").AllowAnonymous();
                });

                app.UseHttpsRedirection();
                //app.MapControllers().RequireRateLimiting(rateLimitingConfig.PolicyName);

                Timer timer = new Timer((Object o) =>
                {
                    Log.Logger.Information($".NET runtime version: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}");
                    Log.Logger.Information("{message}", "**** Service Gateway is now running ****");
                }, null, 20000, Timeout.Infinite);

                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Logger.Error(ex, "Error in Main of Program.cs");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        static void ProcessExitHandler(object sender, EventArgs e, WebApplication app)
        {
            //This method is called whenever the API is getting closed from running state
            //Console.WriteLine("Application Closing");
            //var authenticateUserService = app.Services.GetService<IAuthenticateUserService>();
            //authenticateUserService.ConnectionStatusDisconnected();
            //Console.ReadKey();
        }

        private static void InstantiateServices(WebApplication app)
        {
            using (var op = Operation.Begin("Services kafka connection with Greenfield services"))   //to log time of completion
            {
                try
                {

                    //Get the Instance of IKafkaManager as we need to Initialize and CreateTopics at application start
                    var kafkaManager = app.Services.GetService<IKafkaManager>();
                    kafkaManager.Initialize(app.Configuration.GetValue<string>("KafkaConfigPath"));
                    kafkaManager.CreateTopics().Wait();

                    var authenticateUserService = app.Services.GetService<IAuthenticateUserService>();
                    authenticateUserService.Initialize();

                    var blotterService = app.Services.GetService<IBlotterService>();
                    blotterService.Initialize();

                    var commonDataService = app.Services.GetService<ICommonDataService>();
                    commonDataService.Initialize();

                    var liveFeedService = app.Services.GetService<ILiveFeedService>();
                    liveFeedService.Initialize();

                    var securityValidationService = app.Services.GetService<ISecurityValidationService>();
                    securityValidationService.Initialize();

                    var tradingService = app.Services.GetService<ITradingService>();
                    tradingService.Initialize();

                    var watchlistDataService = app.Services.GetService<IWatchlistDataService>();

                    var layoutService = app.Services.GetService<ILayoutService>();
                    layoutService.Initialize();

                    var complianceService = app.Services.GetService<IComplianceService>();
                    complianceService.Initialize();

                    var rtpnlService = app.Services.GetService<IRtpnlService>();
                    rtpnlService.Initialize();

                    var openfinManagerService = app.Services.GetService<IOpenfinManagerService>();
                    openfinManagerService.Initialize();

                    op.Complete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Log.Logger.Error(ex, "Error in InstantiateServices of Program.cs");
                    op.Abandon();
                }
            }
        }

        private static void SwaggerConfiguration(WebApplicationBuilder builder)
        {
            try
            {
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(options =>
                {
                    var bearerSecurityDefinition = builder.Configuration.GetSection("BearerSecurityDefinition").Get<OpenApiSecurityScheme>();
                    var tokenSaltSecurityDefinition = builder.Configuration.GetSection("TokenSaltSecurityDefinition").Get<OpenApiSecurityScheme>();
                    options.AddSecurityDefinition("Bearer", bearerSecurityDefinition);
                    options.AddSecurityDefinition("tokenSalt", tokenSaltSecurityDefinition);
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
                    },
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "tokenSalt" }
                        },
                        new string[] {}
                    }
                    });

                    options.OperationFilter<CustomHeaderOperationFilter>();
                    options.AddSignalRSwaggerGen();
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Logger.Error(ex, "Error in SwaggerConfiguration of Program.cs");
            }
        }

        private static void AddSingletonServices(WebApplicationBuilder builder)
        {
            try
            {
                builder.Services.AddMemoryCache();
                builder.Services.AddSingleton<AppMemoryCache>();
                builder.Services.AddSingleton<ServiceHealthStatusStore>();

                builder.Services.AddSingleton<CustomRateLimitingMiddleware>();
                builder.Services.AddSingleton<GlobalExceptionMiddleware>();
                builder.Services.AddSingleton<IAuthenticateUserService, AuthenticateUserService>();
                builder.Services.AddSingleton<ICommonDataService, CommonDataService>();
                builder.Services.AddSingleton<ITradingService, TradingService>();
                builder.Services.AddSingleton<IBlotterService, BlotterService>();
                builder.Services.AddSingleton<ILiveFeedService, LiveFeedService>();
                builder.Services.AddSingleton<IWatchlistDataService, WatchlistDataService>();
                builder.Services.AddSingleton<ILayoutService, LayoutService>();

                builder.Services.AddSingleton<IKafkaManager, KafkaManager>();

                builder.Services.AddSingleton<ISecurityValidationService, SecurityValidationService>();
                builder.Services.AddTransient<IValidationHelper, ValidationHelper>();
                builder.Services.AddTransient<ITokenService, TokenService>();
                builder.Services.AddTransient<TokenDecrypterMiddleware>();
                builder.Services.AddTransient<TokenManagerMiddleware>();
                builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                builder.Services.AddSingleton<IReportsPortalService, ReportsPortalService>();
                builder.Services.AddSingleton<IComplianceService, ComplianceService>();
                builder.Services.AddSingleton<IRtpnlService, RtpnlService>();
                builder.Services.AddSingleton<IOpenfinManagerService, OpenfinManagerService>();
                builder.Services.AddSingleton<IHubManager, HubClientConnectionManager>();
                builder.Services.AddSingleton<IHubManagerRTPNL, HubClientConnectionManagerRTPNL>();
                builder.Services.AddSingleton<IHubManagerRTPNLUpdates, HubClientConnectionManagerRTPNLUpdates>();
                builder.Services.AddSingleton<IPowerBiEmbedReportService, PowerBiEmbedReportService>();
                builder.Services.AddSingleton<ServiceHealthStatusStore>();
                builder.Services.AddHostedService<ServiceHealthStatusSubscriber>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Logger.Error(ex, "Error in AddSingletonServices of Program.cs");
            }
        }


        /// <summary>
        /// This is to block the metadata request (IP or URL) based on the configuration provided in appsettings.json.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static Func<HttpContext, Func<Task>, Task> BlockMetaDataRequest(IConfiguration configuration)
        {
            var blockedHost = configuration[REQUEST_FILTERING_BLOCKHOST];
            var blockedPathsConfig = configuration[REQUEST_FILTERING_BLOCKPATHS];

            var blockedPaths = (blockedPathsConfig ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToArray();

            return async (context, next) =>
            {
                var host = context.Request.Host.Host;
                var path = context.Request.Path;

                if (blockedHost != null &&
                    host.Equals(blockedHost, StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("Request IP is blocked");
                    return;
                }

                if (blockedPaths.Length > 0 &&
                    blockedPaths.Any(p => path.StartsWithSegments(p, StringComparison.OrdinalIgnoreCase)))
                {
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("Request path is blocked");
                    return;
                }
                await next();
            };
        }

        //private static RateLimitPartition<string> CreateRateLimitPartition(HttpContext context, RateLimitingOptions config)
        //{
        //    var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        //    var path = context.Request.Path.ToString().ToLowerInvariant();
        //    var key = $"{ip}:{path}";

        //    return RateLimitPartition.GetTokenBucketLimiter(key, _ => new TokenBucketRateLimiterOptions
        //    {
        //        TokenLimit = config.TokenLimit,
        //        TokensPerPeriod = config.TokensPerPeriod,
        //        ReplenishmentPeriod = TimeSpan.FromSeconds(config.ReplenishmentPeriodInSeconds),
        //        AutoReplenishment = true,
        //        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
        //        QueueLimit = config.QueueLimit
        //    });
        //}
        //private static ValueTask HandleRateLimitRejection(OnRejectedContext context, string rejectionMessage, CancellationToken token)
        //{
        //    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        //    return new ValueTask(context.HttpContext.Response.WriteAsync(rejectionMessage, token));
        //}

    }
}