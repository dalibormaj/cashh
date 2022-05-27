using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using NJsonSchema.Generation;
using NSwag;
using System;
using System.Reflection;
using Victory.Auth;
using Victory.VCash.Api.Controllers;
using Victory.VCash.Api.Controllers.Dtos.Requests;
using Victory.VCash.Api.Mappers;
using Victory.VCash.Infrastructure.Common;

namespace Victory.VCash.Api.Extensions
{
    public static class ApiExtensions
    {
        private static void AddSwagger(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var groupProvider = serviceProvider.GetRequiredService<IApiDescriptionGroupCollectionProvider>();

            foreach (var group in groupProvider.ApiDescriptionGroups.Items)
            {
                services.AddOpenApiDocument(document =>
                {
                    //Grouping
                    document.DocumentName = $"{group.GroupName}";
                    document.Title = Assembly.GetExecutingAssembly().GetName().Name;
                    document.ApiGroupNames = new[] { group.GroupName };

                    if (group.GroupName.Equals(ControllerGroupName.APP, StringComparison.OrdinalIgnoreCase))
                    {
                        document.AddSecurity(AuthSchema.BEARER, new OpenApiSecurityScheme
                        {
                            Description = @"Guardian - Authorization header using the Bearer scheme. Enter your token in the text input below.
                                            Example: 'Bearer kJ8aCbzuMRezSblLVmQlMSZB1ajPS5PtT23hS8QIuqBpYphHx4izc'",
                            Name = HeaderNames.Authorization,
                            In = OpenApiSecurityApiKeyLocation.Header,
                            Type = OpenApiSecuritySchemeType.Http,
                            Scheme = AuthSchema.BEARER
                        });
                    }

                    if (group.GroupName.Equals(ControllerGroupName.ADMIN, StringComparison.OrdinalIgnoreCase))
                    {
                        document.AddSecurity(AuthSchema.AZURE_AD, new OpenApiSecurityScheme
                        {
                            Description = @"AzureAd - Authorization header using the AzureAd scheme. Enter your token in the text input below.
                                            Example: 'AzureAd kJ8aCbzuMRezSblLVmQlMSZB1ajPS5PtT23hS8QIuqBpYphHx4izc'",
                            Name = HeaderNames.Authorization,
                            In = OpenApiSecurityApiKeyLocation.Header,
                            Type = OpenApiSecuritySchemeType.ApiKey,
                            Scheme = AuthSchema.AZURE_AD
                        });
                    }                   
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
            services.AddTransient<IValidator<DepositRequest>, DepositRequestValidator>();
            services.AddTransient<IValidator<PayoutRequest>, PayoutRequestValidator>();
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
