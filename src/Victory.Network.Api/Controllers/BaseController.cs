using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Victory.Network.Infrastructure.Common;

namespace Victory.Network.Api.Controllers
{
    [ApiController]
    public abstract class BaseController : Controller
    {
        private IGlobalValidator _globalValidator;
        private IMapper _mapper;

        protected IGlobalValidator GlobalValidator => _globalValidator ?? (_globalValidator = HttpContext.RequestServices.GetService<IGlobalValidator>());
        protected IMapper Mapper => _mapper ?? (_mapper = HttpContext.RequestServices.GetService<IMapper>());
    }
}
