using AutoMapper;
using Victory.VCash.Api.Controllers.AgentApp.Dtos.Responses;
using Victory.VCash.Domain.Query;

namespace Victory.VCash.Api.Mappers
{
    public class AgentMapperProfile : Profile
    {
        public AgentMapperProfile()
        {
            CreateMap<CashierWithDetails, _CashierDto>().ReverseMap();
        }
    }
}
