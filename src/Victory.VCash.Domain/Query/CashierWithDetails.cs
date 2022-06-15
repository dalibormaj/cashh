using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Domain.Enums;

namespace Victory.VCash.Domain.Query
{
    public class CashierWithDetails
    {
        public string CashierId { get; init; }
        public Guid? ParentAgentId { get; set; }
        public int? VenueId { get; set; }
        public CashierStatus? CashierStatusId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pin { get; set; }
        public string VenueName { get; set; }
        public string VenueCity { get; set; }
        public string VenueMunicipality { get; set; }
        public string VenueAddress { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyRegistrationNumber { get; set; }
        public string CompanyTaxNumber { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyMunicipality { get; set; }
        public string CompanyAddress { get; set; }
    }
}
