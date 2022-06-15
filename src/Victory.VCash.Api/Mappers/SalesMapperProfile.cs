using AutoMapper;
using System;
using System.Collections.Generic;
using Victory.VCash.Api.Controllers.SalesApp.Dtos.Requests;
using Victory.VCash.Application.Services.AgentService.Inputs;
using Victory.VCash.Domain.Enums;
using Victory.VCash.Domain.Models;
using Victory.VCash.Infrastructure.Common;
using Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Enums;
using Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Requests;

namespace Victory.VCash.Api.Mappers
{
    public class SalesMapperProfile : Profile
    {
        public SalesMapperProfile()
        {
            CreateMap<_CompanyDto, Company>().ReverseMap();
            CreateMap<_VenueDto, Venue>().ReverseMap();
            CreateMap<_RegisterAgentRequest, RegisterAgentInput>().ConvertUsing((src, dest, context) => new RegisterAgentInput()
            {
                Agent = new Agent()
                {
                    AgentStatusId = AgentStatus.DRAFT,
                    FirstName = src.FirstName,
                    LastName = src.LastName,
                    Email = src.Email,
                    PhoneNumber = src.PhoneNumber,
                    RefferalCode = $"V{new Random().Next(10000, 99999)}" //TODO!!! naci bolji nacin, ovako mogu da se ponove vrednosti
                },
                Venues = src.Company.Venues != null? context.Mapper.Map<List<Venue>>(src.Company.Venues) : null,
                Company = context.Mapper.Map<Company>(src.Company),
                BankAccounts = new List<BankAccount>() 
                { 
                    new BankAccount()
                    {
                        AccountNumber = src.Company.BankAccountNumber
                    }
                }
            });
        }
    }
}

