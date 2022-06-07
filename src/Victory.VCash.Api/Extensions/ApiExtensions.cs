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
using Victory.VCash.Api.Controllers.CashierApp;
using Victory.VCash.Api.Controllers.CashierApp.Dtos.Requests;
using Victory.VCash.Api.Mappers;
using Victory.VCash.Infrastructure.Common;

namespace Victory.VCash.Api.Extensions
{
    public static class ApiExtensions
    {
        private static void AddSwagger(this IServiceCollection services)
        {
            const string BEARER = "Bearer";
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

                    document.AddSecurity(BEARER, new OpenApiSecurityScheme
                    {
                        Description = @"Guardian - Authorization header using the Bearer scheme. Enter your token in the text input below.
                                        Example: 'Bearer kJ8aCbzuMRezSblLVmQlMSZB1ajPS5PtT23hS8QIuqBpYphHx4izc'",
                        Name = HeaderNames.Authorization,
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Type = OpenApiSecuritySchemeType.Http,
                        Scheme = BEARER
                    });      
                });
            }
        }

        private static void AddMappers(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile<AuthMapperProfile>();
                mc.AddProfile<CashierMapperProfile>();
                mc.AddProfile<SalesMapperProfile>();
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
