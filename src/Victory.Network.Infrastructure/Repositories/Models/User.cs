using System;
using Victory.Network.Infrastructure.Repositories.Enums;

namespace Victory.Network.Infrastructure.Repositories.Models
{
    public class User
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string ExternalId { get; set; }
        public string UserName { get; set; }
        public bool IsTest { get; set; }
        public UserType UserType { get; set; }
        public UserStatus UserStatus { get; set; }
        public int? ParentId { get; set; }
        public Brand Brand { get; set; }
        public string AuditCreatedBy { get; set; }
        public string AuditLastUpdatedBy { get; set; }
        public DateTime? AuditCreatedOn { get; set; }
        public DateTime? AuditLastUpdateOn { get; set; }
        public bool AuditChangeFlag { get; set; }
        public string Department { get; set; }
        public int ActualUserId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string CurrencyISOCode { get; set; }
        public UserCategory Category { get; set; }
    }
}
