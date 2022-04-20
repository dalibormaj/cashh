using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using NSwag;
using System.Reflection;
using Victory.Auth;
using Victory.Network.Api.Dtos.Requests;
using Victory.Network.Api.Mappers;
using Victory.Network.Infrastructure.Common;

namespace Victory.Network.Api.Extensions
{
    public static class ApiExtensions
    {
        private static void AddSwagger(this IServiceCollection services)
        {
            services.AddApiVersioning();
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "VVV"; //Major and minor version... see: https://github.com/microsoft/aspnet-api-versioning/wiki/Version-Format#custom-api-version-format-strings
                options.SubstituteApiVersionInUrl = true;
            });
            var provider = services.BuildServiceProvider()
                                   .GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var versionDesc in provider.ApiVersionDescriptions)
            {
                services.AddOpenApiDocument(document =>
                {
                    //Versioning
                    document.DocumentName = $"v{versionDesc.GroupName}";
                    document.Title = Assembly.GetExecutingAssembly().GetName().Name;
                    document.Version = versionDesc.ApiVersion.ToString();
                    document.ApiGroupNames = new[] { versionDesc.GroupName };
                    document.AddSecurity(AuthSchema.BEARER, new OpenApiSecurityScheme
                    {
                        Description = @"Authorization header using the Bearer scheme. Enter your token in the text input below.
                                        Example: 'kJ8aCbzuMRezSblLVmQlMSZB1ajPS5PtT23hS8QIuqBpYphHx4izc'",
                        Name = HeaderNames.Authorization,
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Type = OpenApiSecuritySchemeType.Http,
                        Scheme = AuthSchema.BEARER
                    });

                    document.AddSecurity(AuthSchema.AZURE_AD, new OpenApiSecurityScheme
                    {
                        Description = @"Azure AD",
                        Name = HeaderNames.Authorization,
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Scheme = AuthSchema.AZURE_AD
                    });
                });
            }
        }

        private static void AddMappers(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserControllerProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        private static void AddValidators(this IServiceCollection services)
        {
            services.AddTransient<IGlobalValidator, GlobalValidator>();
            services.AddTransient<IValidator<RegisterUserRequest>, RegisterUserRequestValidator>();
        }

        public static void AddVictoryApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwagger();
            services.AddValidators();
            services.AddMappers();
            services.AddVictoryAuth(configuration);
        }
    }
}
