using System.Collections.Generic;

namespace Victory.VCash.Api.Controllers.AgentApp.Dtos.Responses
{
    public class _GetCashiersResponse : BaseResponse
    {
        public List<_CashierDto> Cashiers { get; set; }
    }

    public class _CashierDto
    {
        public string CashierId { get; set; }
        public string StatusCode { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int VenueId { get; set; }
        public string VenueName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}
