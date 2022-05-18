﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Victory.Auth;
using Victory.VCash.Api.AdminControllers.Dtos.Requests;
using Victory.VCash.Api.AdminControllers.Dtos.Responses;
using Victory.VCash.Api.Controllers;

namespace Victory.VCash.Api.AdminControllers
{
    [ApiExplorerSettings(GroupName = ControllerGroupName.ADMIN)]
    [Route("admin/agent")]
    public class AgentController : BaseController
    {
        public AgentController()
        {

        }

        [HttpGet]
        [Route("")]
        [Authorize(AuthSchema.AZURE_AD)]
        public async Task<_RegisterAgentResponse> GetAdmin()
        {
            var user = HttpContext.User.GetUserName();
            return new _RegisterAgentResponse();
        }

        [HttpPost]
        [Route("register")]
        [Authorize(AuthSchema.AZURE_AD)]
        public async Task<_RegisterAgentResponse> RegisterAgent(_RegisterAgentRequest request)
        {
            return new _RegisterAgentResponse();
        }

    }
}