using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Domain.Enums;

namespace Victory.VCash.Domain.Models
{
    public class Cashier : BaseDomainModel
    {
        public string CashierId { get; init; }
        public Guid? ParentAgentId { get; set; }
        public int? VenueId { get; set; }
        public CashierStatus? CashierStatusId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pin { get; set; }
    }
}
