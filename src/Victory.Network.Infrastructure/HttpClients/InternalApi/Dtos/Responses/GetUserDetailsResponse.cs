using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.Network.Infrastructure.HttpClients.InternalApi.Dtos.Responses
{

    public class GetUserDetailsResponse
    {
        public string UserId { get; set; }
        public bool IsDeleted { get; set; }
        public string ExternalId { get; set; }
        public string Username { get; set; }
        public bool IsTest { get; set; }
        public UserType UserType { get; set; }
        public UserStatus UserStatus { get; set; }
        public int ParentID { get; set; }
        public Brand Brand { get; set; }
        public string AuditCreatedBy { get; set; }
        public string AuditLastUpdatedBy { get; set; }
        public DateTime AuditCreatedOn { get; set; }
        public DateTime AuditLastUpdateOn { get; set; }
        public bool AuditChangeFlag { get; set; }
        public string Department { get; set; }
        public int ActualUserID { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string CurrencyISOCode { get; set; }
        public UserCategory UserCategory { get; set; }
        public UserDetail UserDetail { get; set; }
        public List<UserAddressDetail> UserAddressDetails { get; set; }
        public SecurityDetails SecurityDetails { get; set; }
        public OtpSecurityDetails OtpSecurityDetails { get; set; }
        public List<PhoneContactDetail> PhoneContactDetails { get; set; }
        public List<_Email> Emails { get; set; }
        public List<ExtraDetail> ExtraDetails { get; set; }
        public SiteDiscoveryMethod SiteDiscoveryMethod { get; set; }
    }

    public class UserType
    {
        public int ParentId { get; set; }
        public int Priority { get; set; }
        public bool IsSystem { get; set; }
        public bool IsAbstract { get; set; }
        public string ShortCode { get; set; }
        public bool IsInternalUser { get; set; }
        public int Id { get; set; }
        public Brand Brand { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class Brand
    {
        public int Id { get; set; }
        public string DefaultTimeZone { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int StatusId { get; set; }
        public bool IsTest { get; set; }
        public string HostHeader { get; set; }
    }

    public class UserStatus
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public bool IsActive { get; set; }
        public Brand Brand { get; set; }
    }


    public class UserCategory
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public Brand Brand { get; set; }
        public string HexColorRepresentation { get; set; }
        public string Name { get; set; }
    }

  
    public class UserDetail
    {
        public int Id { get; set; }
        public DateTime BirthDate { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CountryCode { get; set; }
        public string RegistrationIPAddress { get; set; }
        public string LanguageCode { get; set; }
        public string Comment { get; set; }
        public GenderType GenderType { get; set; }
        public TitleType TitleType { get; set; }
    }

    public class GenderType
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystem { get; set; }
        public string DescriptionLong { get; set; }
        public Brand Brand { get; set; }
    }

    public class TitleType
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystem { get; set; }
        public string DescriptionLong { get; set; }
        public Brand Brand { get; set; }
    }


    public class SecurityDetails
    {
        public SecurityQuestion SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public string Password { get; set; }
        public bool isApproved { get; set; }
        public DateTime LastActivityDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public bool IsOnLine { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime LastLockedOutDate { get; set; }
        public int FailedPasswordAnswerAttemptCount { get; set; }
        public DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }
        public int FailedPasswordAttemptCount { get; set; }
        public DateTime FailedPasswordAttemptWindowStart { get; set; }
        public int SecurityQuestionId { get; set; }
    }

    public class SecurityQuestion
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystem { get; set; }
        public string DescriptionLong { get; set; }
        public Brand Brand { get; set; }
    }

    public class OtpSecurityDetails
    {
        public string Password { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    public class SiteDiscoveryMethod
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystem { get; set; }
        public string DescriptionLong { get; set; }
        public Brand Brand { get; set; }
    }


    public class UserAddressDetail
    {
        public string Id { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public string Province { get; set; }
        public string Village { get; set; }
        public AddressType AddressType { get; set; }
    }

    public class AddressType
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystem { get; set; }
        public string DescriptionLong { get; set; }
        public string Type { get; set; }
        public int Id { get; set; }
    }

    public class PhoneContactDetail
    {
        public string Id { get; set; }
        public string ContactNumber { get; set; }
        public PhoneContactType PhoneContactType { get; set; }
        public PhoneContactVerificationStatus PhoneContactVerificationStatus { get; set; }
        public DateTime LastStatusChangeDate { get; set; }
    }

    public class PhoneContactType
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystem { get; set; }
        public string DescriptionLong { get; set; }
        public int Id { get; set; }
    }

    public class PhoneContactVerificationStatus
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string ResourceKey { get; set; }
        public string DisplayName { get; set; }
    }

    public class _Email
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public EmailType EmailTypeId { get; set; }
    }

    public class EmailType
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystem { get; set; }
        public string DescriptionLong { get; set; }
        public Brand Brand { get; set; }
    }


    public class ExtraDetail
    {
        public int ID { get; set; }
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
    }

}


