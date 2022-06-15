using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag.AspNetCore;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Victory.VCash.Api.Extensions;
using Victory.VCash.Api.Middlewares;
using Victory.VCash.Api.Workers;
using Victory.VCash.Application.Extensions;
using Victory.VCash.Infrastructure.Errors;
using Victory.VCash.Infrastructure.Extensions;
using Victory.VCash.Infrastructure.Resources;

namespace Victory.VCash
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private readonly string _corsPolicyName = "CorsPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(options =>
                    {
                        options.LowercaseUrls = true;
                        options.LowercaseQueryStrings = true;
                    })
                    .AddHttpContextAccessor()
                    .AddCors(opt =>
                    {
                        opt.AddPolicy(name: _corsPolicyName, builder =>
                        {
                            builder.AllowAnyOrigin()
                                   .AllowAnyHeader()
                                   .AllowAnyMethod();
                        });
                    })
                    .AddControllers(options => 
                    {
                        options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
                    })
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                        options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    })
                    .ConfigureApiBehaviorOptions(x =>
                    {
                        x.InvalidModelStateResponseFactory = context =>
                        {
                            var errorMessage = GetErrorMessage(context);
                            throw new VCashBadRequestException(ErrorCode.BAD_REQUEST, errorMessage);
                        };
                    });

            services.AddMvc()
                    .AddDataAnnotationsLocalization(options =>
                    {
                        options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResource));
                    });

            //add core logic
            services.AddVictoryApi(Configuration);
            services.AddVictoryApplication(Configuration);
            services.AddVictoryInfrastructure(Configuration);

            //background services
            services.AddVictoryWorkers(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
                              IWebHostEnvironment env, 
                              IApiDescriptionGroupCollectionProvider groupProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseOpenApi();
                app.UseSwaggerUi3(options =>
                {
                    //build a swagger endpoint for each discovered group
                    foreach (var group in groupProvider.ApiDescriptionGroups.Items)
                    {
                        options.SwaggerRoutes.Add(new SwaggerUi3Route($"{group.GroupName}", $"/swagger/{group.GroupName}/swagger.json"));
                    }
                });

            };

            app.UseCors(_corsPolicyName);

            //note: order is important
            app.UseMiddleware<LanguageMiddleware>();
            app.UseMiddleware<UnauthorizedMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

        /// <summary>
        /// Get low-level errors.These errors occur first and don't even go through the other validation mechanisms like FluentValidataions
        /// Example: Request expects int but user passes string, wrong Enum conversions etc. 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetErrorMessage(ActionContext context)
        {
            string errorMessage = string.Empty;
            foreach (var item in context.ModelState)
            {
                var key = item.Key;
                var rawValue = item.Value.RawValue;
                var errors = item.Value.Errors;
                if ((errors?.Count ?? 0) > 0)
                {
                    errorMessage = $"Field '{key}' is missing or contains an invalid value";

                    if (key == "$")
                        errorMessage = "invalid JSON format";
                    break;
                }
            }

            return errorMessage;
        }
    }
    
    //custom converter for UTC (with Z at the end)
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            //Set UTC format
            writer.WriteStringValue(value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
        }
    }


    //Routing rule - add dash between each word in controller name
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            // Slugify value
            return value == null ? null : Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }


}
