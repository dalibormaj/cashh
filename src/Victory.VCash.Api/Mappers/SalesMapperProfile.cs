using AutoMapper;
using System;
using System.Collections.Generic;
using Victory.VCash.Api.Controllers.AgentApp.Dtos.Requests;
using Victory.VCash.Application.Services.AgentService.Inputs;
using Victory.VCash.Domain.Enums;
using Victory.VCash.Domain.Models;
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
                EmailVerificationUrl = src.EmailVerificationUrl,
                Agent = new Agent()
                {
                    AgentStatusId = AgentStatus.PENDING_APPROVAL,
                    Email = src.Email,
                    PhoneNumber = src.PhoneNumber,
                    RefferalCode = $"V{new Random().Next(10000, 99999)}" //TODO!!! naci bolji nacin, ovako mogu da se ponove vrednosti
                },
                Venues = src.Company.Venues != null? context.Mapper.Map<List<Venue>>(src.Company.Venues) : null,
                Company = context.Mapper.Map<Company>(src.Company)
            });

            CreateMap<RegisterAgentInput, RegisterUserRequest>().ConvertUsing((src, dest, context) => new RegisterUserRequest()
            {   EmailVerificationUrl = src.EmailVerificationUrl,
                Email = src.Agent?.Email,
                UserTypeCode = "AGENT",
                Password = "iTest123!", 
                PhoneContacts = new[]{ new PhoneContact()
                {
                    PhoneContactTypeCode = "MB",
                    Prefix = "+381",
                    Number = src.Agent?.PhoneNumber
                }},
                ExtraRegistrationValues = new ExtraRegistrationValues()
                {

                    ReceiveMarketingMessages = true
                }
            });
            
        }
    }
}
