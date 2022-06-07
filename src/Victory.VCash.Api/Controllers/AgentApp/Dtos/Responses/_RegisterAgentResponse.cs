using System.Collections.Generic;
using Victory.VCash.Api.Controllers.CashierApp.Dtos.Responses;

namespace Victory.VCash.Api.Controllers.AgentApp.Dtos.Responses
{
    public class _RegisterAgentResponse : BaseResponse
    {
        public string AgentId { get; set; }
        public int CompanyId { get; set; }
        public List<_VenueResponseDto> Venues { get; set; }
    }

    public class _VenueResponseDto
    {
        public int VenueId { get; set; }
        public string VenueName { get; set; }
    }
}
