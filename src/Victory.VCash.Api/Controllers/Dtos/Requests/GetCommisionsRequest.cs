using System;

namespace Victory.VCash.Api.Controllers.Dtos.Requests
{
    public class GetCommisionsRequest
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
