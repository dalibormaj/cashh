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
    public class CashierMapperProfile : Profile
    {
        public CashierMapperProfile()
        {
            CreateMap<GetUserOutput, GetUserResponse>().ForMember(x => x.BirthDate, opt => opt.MapFrom((src, dest) =>
            {
                return src.BirthDate?.ToShortDateString() ?? null;
            }));
            CreateMap<Transaction, TransactionDto>().ForMember(x => x.Date, opt => opt.MapFrom(src => src.InsertDate)).ReverseMap();
            CreateMap<MoneyTransfer, MoneyTransferDto>().ForMember(x => x.StatusCode, opt => opt.MapFrom(src => src.MoneyTransferStatusId.ToString()))
                                                        .ForMember(x => x.Date, opt => opt.MapFrom(src => src.InsertDate)).ReverseMap();
        }
    }
}
