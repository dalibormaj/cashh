using AutoMapper;
using System;
using System.Linq;
using Victory.VCash.Api.Controllers.Auth.Dtos.Responses;
using Victory.VCash.Api.Controllers.CashierApp.Dtos;
using Victory.VCash.Api.Controllers.CashierApp.Dtos.Responses;
using Victory.VCash.Application.Services.AuthService.Results;
using Victory.VCash.Application.Services.UserService.Outputs;
using Victory.VCash.Domain.Models;
using Victory.VCash.Infrastructure.HttpClients.InternalApi.Dtos.Responses;

namespace Victory.VCash.Api.Mappers
{
    public class AuthMapperProfile : Profile
    {
        public AuthMapperProfile()
        {
            CreateMap<ValidateDeviceTokenResult, ValidateDeviceTokenResponse>().ReverseMap();
        }

        //private void MapGetUserDetailsResponse()
        //{
            //CreateMap<GetUserDetailsResponse, GetUserResponse>().ForMember(x => x.UserId, opt => opt.MapFrom(src => src.UserId))
            //                                                    .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.Username))
            //                                                    .ForMember(x => x.Name, opt => opt.MapFrom((src, dest) =>
            //                                                    {
            //                                                        return src.UserDetail?.Name ?? null;
            //                                                    }))
            //                                                    .ForMember(x => x.LastName, opt => opt.MapFrom((src, dest) =>
            //                                                    {
            //                                                        return src.UserDetail?.Surname ?? null;
            //                                                    }))
            //                                                    .ForMember(x => x.CitizenId, opt => opt.MapFrom(src => src.ExtraDetails
            //                                                                                                            .FirstOrDefault(x => "Blinking.CitizenId".Equals(x.PropertyName, StringComparison.OrdinalIgnoreCase))
            //                                                                                                            .PropertyValue))
            //                                                    .ForMember(x => x.StatusCode, opt => opt.MapFrom(src => src.UserStatus.Code))
            //                                                    .ForMember(x => x.BirthDate, opt => opt.MapFrom(src => src.UserDetail.BirthDate))
            //                                                    .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Emails.FirstOrDefault().Email))
            //                                                    .ForMember(x => x.MobilePhone, opt => opt.MapFrom((src,dest) =>
            //                                                    {
            //                                                        return src.PhoneContactDetails.FirstOrDefault()?.ContactNumber ?? null;
            //                                                    }));




            ////Full response
            //CreateMap<UserAddressDetail, AddressDto>().ForMember(x => x.Street, opt => opt.MapFrom(src => src.Line1));
            //CreateMap<PhoneContactDetail, PhoneDto>().ForMember(x => x.PhoneNumber, opt => opt.MapFrom(src => src.ContactNumber))
            //                                         .ForMember(x => x.StatusCode, opt => opt.MapFrom(src => src.PhoneContactVerificationStatus.Code))
            //                                         .ForMember(x => x.StatusDescription, opt => opt.MapFrom(src => src.PhoneContactVerificationStatus.Description));
            //CreateMap<GetUserDetailsResponse, GetUserResponse>().ForMember(x => x.ReceiveMarketingMessages, opt => opt.MapFrom(src => src.ExtraDetails
            //                                                                                                                          .FirstOrDefault(x => "ReceiveMarketingMessages".Equals(x.PropertyName, StringComparison.OrdinalIgnoreCase))
            //                                                                                                                          .PropertyValue))
            //                                                 .ForMember(x => x.PoliticallyExposed, opt => opt.MapFrom(src => src.ExtraDetails
            //                                                                                                                    .FirstOrDefault(x => "IsPoliticallyExposed".Equals(x.PropertyName, StringComparison.OrdinalIgnoreCase))
            //                                                                                                                    .PropertyValue))
            //                                                 .ForMember(x => x.CitizenId, opt => opt.MapFrom(src => src.ExtraDetails
            //                                                                                                           .FirstOrDefault(x => "Blinking.CitizenId".Equals(x.PropertyName, StringComparison.OrdinalIgnoreCase))
            //                                                                                                           .PropertyValue))
            //                                                 .ForMember(x => x.StatusCode, opt => opt.MapFrom(src => src.UserStatus.Code))
            //                                                 .ForMember(x => x.BirthDate, opt => opt.MapFrom(src => src.UserDetail.BirthDate))
            //                                                 .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Emails.FirstOrDefault().Email))
            //                                                 .ForMember(x => x.Addresses, opt => opt.MapFrom(src => src.UserAddressDetails))
            //                                                 .ForMember(x => x.Phones, opt => opt.MapFrom(src => src.PhoneContactDetails))
            //                                                 .ForMember(x => x.EmailVerified, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.ExtraDetails
            //                                                                                                                   .FirstOrDefault(x => "EmailVerifiedDate".Equals(x.PropertyName,
            //                                                                                                                                                                   StringComparison.OrdinalIgnoreCase))
            //                                                                                                                   .PropertyValue)));

       // }
    }
}
