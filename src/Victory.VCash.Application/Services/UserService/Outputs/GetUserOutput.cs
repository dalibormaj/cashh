using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Application.Services.UserService.Outputs
{
    public class GetUserOutput
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string CitizenId { get; set; }
        public string StatusCode { get; set; }
        public string UserTypeCode { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
