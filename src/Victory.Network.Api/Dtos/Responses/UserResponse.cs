using System;
using System.Collections.Generic;

namespace Victory.Network.Api.Dtos.Responses
{
    public class UserResponse : BaseResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public DateTime BirthDate { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; } 
        public string StatusCode { get; set; }
        public string GenderCode { get; set; }  
        public string TitleCode { get; set; }   
        public string CountryCode { get; set; } 
        public string LanguageCode { get; set; }   
        public string UserTypeCode {  get; set; }
        public string CitizenId { get; set; }   
        public bool PoliticallyExposed { get; set; }
        public bool ReceiveMarketingMessages { get; set; }   
        public int ParentId { get; set; }
        public List<AddressDto> Addresses { get; set; }
        public List<PhoneDto> Phones { get; set; }
    }

    public class AddressDto
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
    }

    public class PhoneDto
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
    }
}
