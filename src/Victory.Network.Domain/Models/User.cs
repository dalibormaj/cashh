using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.Network.Domain.Models
{
    public class User
    {
        public int UserId { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }    
        public string LastName { get; set; }    
    }
}
