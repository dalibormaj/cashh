using System.ComponentModel;

namespace Victory.Network.Infrastructure.Errors
{
    public enum ErrorCode
    {
        [Description("Ok")]
        OK,
        [Description("Bad request")]
        BAD_REQUEST,
        [Description("Can't validate an empty object")]
        CANT_VALIDATE_EMPTY_OBJECT,
        [Description("Citizen Id missing")]
        CITIZEN_ID_MISSING,
        [Description("Email missing")]
        EMAIL_MISSING,
        [Description("Mobile number missing")]
        MOBILE_NUMBER_MISSING,
        [Description("EmailVerifcationUrl missing")]
        EMAIL_VERIFICATION_URL_MISSING,
        [Description("IsPoliticallyExposed missing")]
        IS_POLITICALLY_EXPOSED_MISSING,
        [Description("ReceiveMarketingMessage missing")]
        RECEIVE_MARKETING_MESSAGES_MISSING
    }
}
