using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Victory.Auth;
using Victory.VCash.Api.Controllers.CashierApp.Dtos.Requests;
using Victory.VCash.Api.Controllers.CashierApp.Dtos.Responses;
using Victory.VCash.Application.Services.MoneyTransferService;
using Victory.VCash.Application.Services.UserService;

namespace Victory.VCash.Api.Controllers.CashierApp
{
    public class UserController : BaseCashierAppController
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

        [HttpGet]
        public async Task<GetUserResponse> GetUser(string identifier)
        {
            var user = await _userService.GetUserAsync(identifier, maskBasicValues: true);
            return Mapper.Map<GetUserResponse>(user);
        }
    }
}
