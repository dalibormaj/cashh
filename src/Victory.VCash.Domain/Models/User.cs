using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victory.VCash.Domain.Enums;

namespace Victory.VCash.Domain.Models
{
    public class User
    {
        public int UserId { get; init; }
        public int? ParentUserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }    
        public string Name { get; set; }    
        public string LastName { get; set; }    
        public string AffiliateCode { get; set; }
        public UserType? UserTypeId { get; set; }
        public UserStatus? UserStatusId { get; set; }
    }
}
