using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Domain.Models
{
    public class BankAccount : BaseDomainModel
    {
        public int? BankAccountId { get; set; }
        public int? CompanyId { get; set; }
        public string Bank { get; set; }
        public string AccountNumber { get; set; }
    }
}
