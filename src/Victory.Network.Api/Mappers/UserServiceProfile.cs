using AutoMapper;
using Victory.Network.Api.Dtos.Responses;
using Victory.Network.Application.Services.UserService.Outputs;

namespace Victory.Network.Api.Mappers
{
    public class UserServiceProfile : Profile
    {
        public UserServiceProfile()
        {
            CreateMap<RegisterUserOutput, RegisterUserResponse>();//.ForMember(x => x.UserId, opt => opt.MapFrom(src => src.UserId))
        }
    }
}
