using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Victory.Auth;
using Victory.VCash.Api.Controllers.Dtos.Requests;
using Victory.VCash.Api.Controllers.Dtos.Responses;
using Victory.VCash.Application.Services.MoneyTransferService;
using Victory.VCash.Application.Services.UserService;

namespace Victory.VCash.Api.Controllers
{
    [Route("user")]
    public class UserController : BaseController
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //[HttpPost]
        //[Route("register")]
        //[Authorize]
        //public async Task<RegisterUserResponse> RegisterUser([FromBody] RegisterUserRequest request)
        //{
        //    GlobalValidator.Validate(request);
        //    var agentId = HttpContext.User.GetId() ?? throw new ArgumentException();
        //    var userId = await _userService.RegisterUserAsync(agentId,
        //                                                      request.CitizenId, 
        //                                                      request.EmailVerificationUrl, 
        //                                                      request.Email, 
        //                                                      request.MobilePhoneNumber, 
        //                                                      (bool)request.ReceiveMarketingMessages, 
        //                                                      (bool)request.IsPoliticallyExposed);
        //    return new RegisterUserResponse() { UserId = userId };
        //}

        //[HttpGet]
        //[Authorize]
        //public async Task<UserResponse> GetUser(string identifier)
        //{
        //    var user = await _userService.GetUser(identifier);
        //    return Mapper.Map<UserResponse>(user);
        //}
    }
}
