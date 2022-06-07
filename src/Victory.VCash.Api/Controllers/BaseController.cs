using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Victory.VCash.Infrastructure.Common;

namespace Victory.VCash.Api.Controllers
{
    [ApiController]
    public abstract class BaseController : Controller
    {
        private IGlobalValidator _globalValidator;
        private IMapper _mapper;

        protected IGlobalValidator GlobalValidator => _globalValidator ?? (_globalValidator = HttpContext.RequestServices.GetService<IGlobalValidator>());
        protected IMapper Mapper => _mapper ?? (_mapper = HttpContext.RequestServices.GetService<IMapper>());
    }

    public static class ControllerGroupName
    {
        public const string CASHIER_APP = "cashier-app";
        public const string AGENT_APP = "agent-app";
        public const string SALES_APP = "sales-app";
        public const string AUTH = "auth";
    }
}
