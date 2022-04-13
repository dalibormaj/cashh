using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Victory.Network.Api.Dtos.Requests;
using Victory.Network.Api.Dtos.Responses;
using Victory.Network.Application.Services.UserService;
using Victory.Network.Infrastructure.Common;

namespace Victory.Network.Api.Controllers
{
    [Route("v{version:apiVersion}/user")]
    public class UserController : BaseController
    {
        private IUserService _userService;
        private IGlobalValidator _validator;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        [Authorize]
        public async Task<RegisterUserResponse> RegisterUser([FromBody] RegisterUserRequest request)
        {
            GlobalValidator.Validate(request);
            var output = await _userService.RegisterUserAsync(request.CitizenId, 
                                                              request.EmailVerificationUrl, 
                                                              request.Email, 
                                                              request.MobilePhoneNumber, 
                                                              (bool)request.ReceiveMarketingMessages, 
                                                              (bool)request.IsPoliticallyExposed);
            return Mapper.Map<RegisterUserResponse>(output);
        }

        [HttpGet]
        [Authorize]
        public async Task GetUser()
        {

        }
    }
}
