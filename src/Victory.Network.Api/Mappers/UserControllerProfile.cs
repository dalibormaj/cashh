using AutoMapper;
using System;
using System.Linq;
using Victory.Network.Api.Dtos.Responses;
using Victory.Network.Application.Services.UserService.Outputs;
using Victory.Network.Infrastructure.HttpClients.InternalApi.Dtos.Responses;

namespace Victory.Network.Api.Mappers
{
    public class UserControllerProfile : Profile
    {
        public UserControllerProfile()
        {
            MapGetUserDetailsResponse();
        }
        private void MapGetUserDetailsResponse()
        {
            //CreateMap<RegisterUserOutput, RegisterUserResponse>();
            CreateMap<UserAddressDetail, AddressDto>().ForMember(x => x.Street, opt => opt.MapFrom(src => src.Line1));
            CreateMap<PhoneContactDetail, PhoneDto>().ForMember(x => x.PhoneNumber, opt => opt.MapFrom(src => src.ContactNumber))
                                                     .ForMember(x => x.StatusCode, opt => opt.MapFrom(src => src.PhoneContactVerificationStatus.Code))
                                                     .ForMember(x => x.StatusDescription, opt => opt.MapFrom(src => src.PhoneContactVerificationStatus.Description));
            CreateMap<GetUserDetailsResponse, UserResponse>().ForMember(x => x.ReceiveMarketingMessages, opt => opt.MapFrom(src => src.ExtraDetails
                                                                                                                                      .FirstOrDefault(x => "ReceiveMarketingMessages".Equals(x.PropertyName, StringComparison.OrdinalIgnoreCase))
                                                                                                                                      .PropertyValue))
                                                             .ForMember(x => x.PoliticallyExposed, opt => opt.MapFrom(src => src.ExtraDetails
                                                                                                                                .FirstOrDefault(x => "IsPoliticallyExposed".Equals(x.PropertyName, StringComparison.OrdinalIgnoreCase))
                                                                                                                                .PropertyValue))
                                                             .ForMember(x => x.CitizenId, opt => opt.MapFrom(src => src.ExtraDetails
                                                                                                                       .FirstOrDefault(x => "Blinking.CitizenId".Equals(x.PropertyName, StringComparison.OrdinalIgnoreCase))
                                                                                                                       .PropertyValue))
                                                             .ForMember(x => x.StatusCode, opt => opt.MapFrom(src => src.UserStatus.Code))
                                                             .ForMember(x => x.BirthDate, opt => opt.MapFrom(src => src.UserDetail.BirthDate))
                                                             .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Emails.FirstOrDefault().Email))
                                                             .ForMember(x => x.Addresses, opt => opt.MapFrom(src => src.UserAddressDetails))
                                                             .ForMember(x => x.Phones, opt => opt.MapFrom(src => src.PhoneContactDetails))
                                                             .ForMember(x => x.EmailVerified, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.ExtraDetails
                                                                                                                               .FirstOrDefault(x => "EmailVerifiedDate".Equals(x.PropertyName,
                                                                                                                                                                               StringComparison.OrdinalIgnoreCase))
                                                                                                                               .PropertyValue)));

        }
    }
}
