using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Victory.VCash.Infrastructure.Common;

namespace Victory.VCash.Api.Controllers
{
    [ApiController]
    [ApiExplorerSettings(GroupName = ControllerGroupName.APP)] //default group
    public abstract class BaseController : Controller
    {
        private IGlobalValidator _globalValidator;
        private IMapper _mapper;

        protected IGlobalValidator GlobalValidator => _globalValidator ?? (_globalValidator = HttpContext.RequestServices.GetService<IGlobalValidator>());
        protected IMapper Mapper => _mapper ?? (_mapper = HttpContext.RequestServices.GetService<IMapper>());
    }

    public static class ControllerGroupName
    {
        public const string APP = "app";
        public const string ADMIN = "admin";
    }
}
