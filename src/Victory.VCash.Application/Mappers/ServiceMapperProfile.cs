using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Domain.Models;
using Victory.VCash.Infrastructure.Common;
using Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Enums;
using Victory.VCash.Infrastructure.HttpClients.PlatformWebSiteApi.Dtos.Requests;

namespace Victory.VCash.Application.Mappers
{
    public class ServiceMapperProfile : Profile
    {
        public ServiceMapperProfile()
        {
            CreateMap<Agent, RegisterUserRequest>().ConvertUsing((src, dest, context) =>
            {
                //format phone number
                MobileNumberHelper.TryFormat(src?.PhoneNumber, out string phoneNumber, action: FormatAction.REMOVE_COUNTRY_CODE);

                return new RegisterUserRequest()
                {
                    Email = src?.Email,
                    UserTypeCode = PL_UserType.AGENT.ToString(),
                    PhoneContacts = new[]{ new PhoneContact()
                    {
                        PhoneContactTypeCode = "MB",
                        Prefix = "+381",
                        Number = phoneNumber
                    }},
                    ExtraRegistrationValues = new ExtraRegistrationValues()
                    {
                        ReceiveMarketingMessages = true
                    }
                };
            });
        }
    }
}
