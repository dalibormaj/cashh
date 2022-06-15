using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Domain.Models
{
    public class Company : BaseDomainModel
    {
        public int? CompanyId { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public string TaxNumber { get; set; }
        public string City { get; set; }
        public string Municipality { get; set; }
        public string Address { get; set; }
        public string GooglePlaceId { get; set; }
        public string GoogleFullAddress { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

    }
}
