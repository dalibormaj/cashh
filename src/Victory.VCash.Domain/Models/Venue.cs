using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Domain.Models
{
    public class Venue
    {
        public int? VenueId { get; set; }
        public int? CompanyId { get; set; }
        public string Name { get; set; }
    }
}
