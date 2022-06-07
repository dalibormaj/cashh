using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Domain.Models
{
    public class Company
    {
        public int? CompanyId { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public string TaxNumber { get; set; }
    }
}
