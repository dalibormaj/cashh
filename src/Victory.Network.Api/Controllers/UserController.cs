using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Victory.Auth;
using Victory.Network.Api.Dtos.Requests;
using Victory.Network.Api.Dtos.Responses;
using Victory.Network.Application.Services.TransactionService;
using Victory.Network.Application.Services.UserService;
using Victory.Network.Infrastructure.Common;

namespace Victory.Network.Api.Controllers
{
    [Route("v{version:apiVersion}/user")]
    public class UserController : BaseController
    {
        private IUserService _userService;
        private ITransactionService _transactionService;
        public UserController(IUserService userService,
                              ITransactionService transactionService)
        {
            _userService = userService;
            _transactionService = transactionService;
        }

        [HttpPost]
        [Route("register")]
        [Authorize]
        public async Task<RegisterUserResponse> RegisterUser([FromBody] RegisterUserRequest request)
        {
            GlobalValidator.Validate(request);
            var agentId = HttpContext.User.GetId() ?? throw new ArgumentException();
            var userId = await _userService.RegisterUserAsync(agentId,
                                                              request.CitizenId, 
                                                              request.EmailVerificationUrl, 
                                                              request.Email, 
                                                              request.MobilePhoneNumber, 
                                                              (bool)request.ReceiveMarketingMessages, 
                                                              (bool)request.IsPoliticallyExposed);
            return new RegisterUserResponse() { UserId = userId };
        }

        [HttpGet]
        [Authorize]
        public async Task<UserResponse> GetUser(string identifier)
        {
            var user = await _userService.GetUser(identifier);
            return Mapper.Map<UserResponse>(user);
        }

        [HttpPost]
        [Route("transaction/deposit")]
        [Authorize]
        public async Task<DepositResponse> Deposit(DepositRequest request)
        {
            var agentId = HttpContext.User.GetId() ?? throw new ArgumentException();
            var transactionId = await _transactionService.TransferFunds(agentId, request.UserId, request.Amount);
            return new DepositResponse() { TransactionId = transactionId };
        }
    }
}
