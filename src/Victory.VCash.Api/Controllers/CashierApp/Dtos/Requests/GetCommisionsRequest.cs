using System;
using System.ComponentModel.DataAnnotations;

namespace Victory.VCash.Api.Controllers.CashierApp.Dtos.Requests
{
    public class GetCommisionsRequest
    {
        [Required]
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
