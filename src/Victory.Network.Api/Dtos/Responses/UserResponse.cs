using System;

namespace Victory.Network.Api.Dtos.Responses
{
    public class UserResponse : BaseResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public Email Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; } 
        public string GenderCode { get; set; }  
        public string TitleCode { get; set; }   
        public string CountryCode { get; set; } 
        public string LanguageCode { get; set; }   
        public string UserTypeCode {  get; set; }
        public string CitizenId { get; set; }   
        public bool IsPoliticallyExposed { get; set; }
        public bool CanReceiveMarketingMessages { get; set; }   
        public int ParentId { get; set; }
        public Address[] Addresses { get; set; }
        public Phone[] Phones { get; set; }
    }

    public class Email
    {
        public string Value { get; set; }   
        public bool IsVerified { get; set; }
    }

    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
    }

    public class Phone
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string StatusCode { get; set; }
    }
}
