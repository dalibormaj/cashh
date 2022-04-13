using System;

namespace Victory.Network.Domain.Model
{
    public class UserDetail
    {
        public int Id { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CountryCode { get; set; }
        public string RegistrationIPAddress { get; set; }
        public string LanguageCode { get; set; }
        public string Comment { get; set; }

        //public GenderTypeModel GenderType { get; set; }
        //public TitleTypeModel TitleType { get; set; }
    }
}
