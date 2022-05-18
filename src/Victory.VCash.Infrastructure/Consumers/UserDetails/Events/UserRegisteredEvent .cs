using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victory.VCash.Infrastructure.Consumers.UserDetails.Events
{
	[MessagePackObject]
	public class UserRegisteredEvent
	{
		[Key("UserId")]
		public int UserId { get; set; }
		[Key("UserName")]
		public string UserName { get; set; }
		[Key("Email")]
		public string Email { get; set; }
		[Key("Name")]
		public string Name { get; set; }
		[Key("LastName")]
		public string LastName { get; set; }
		[Key("UserTypeCode")]
		public string UserTypeCode { get; set; }
		[Key("UserStatusCode")]
		public string UserStatusCode { get; set; }
		[Key("LanguageCode")]
		public string LanguageCode { get; set; }
		[Key("GenderCode")]
		public string GenderCode { get; set; }
		[Key("IsTest")]
		public bool IsTest { get; set; }
		[Key("City")]
		public string City { get; set; }
		[Key("Address")]
		public string Address { get; set; }
		[Key("PhoneContacts")]
		public List<PhoneContact> PhoneContacts { get; set; }
		[Key("ExtraProperties")]
		public List<UserExtraProperty> ExtraProperties { get; set; }
	}

	[MessagePackObject]
	public class PhoneContact
	{
		[Key("Id")]
		public int Id { get; set; }
		[Key("ContactNumber")]
		public string ContactNumber { get; set; }
		[Key("VerificationStatusCode")]
		public string VerificationStatusCode { get; set; }
	}

	[MessagePackObject]
	public class UserExtraProperty
	{
		[Key("Id")]
		public int Id { get; set; }
		[Key("PropertyName")]
		public string PropertyName { get; set; }
		[Key("PropertyValue")]
		public string PropertyValue { get; set; }
	}

}
