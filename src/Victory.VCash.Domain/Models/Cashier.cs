using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Domain.Models
{
    public class Cashier
    {
        public string CashierId { get; init; }
        public string ParentAgentId { get; set; }
        public int ShopId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Pin { get; set; }
    }
}
