using System.Collections.Generic;

namespace Victory.Network.Domain.Model
{
    public static class BlinkingBadgeNames
    {
		public static List<string> Values
		{
			get
			{
				return new()
				{
					"face_auth_ready",
					"blocked_customer",
					"address_confirmed",
					"rejected_customer",
					"contact_data_confirmed",
					"contact_data_confirmed",
					"account_number_confirmed",
					"personal_image_confirmed",
					"government_issued_document_confirmed",
					"user_identity_confirmed"
				};
			}
		}
	}
}
