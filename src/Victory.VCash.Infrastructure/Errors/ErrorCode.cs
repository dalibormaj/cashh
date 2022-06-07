using System.ComponentModel;

namespace Victory.VCash.Infrastructure.Errors
{
    public enum ErrorCode
    {
        [Description("Ok")]
        OK,
        [Description("Bad request: {0}")]
        BAD_REQUEST,
        [Description("System error: {0}")]
        SYSTEM_ERROR,
        [Description("Field '{0}' is mandatory")]
        MANDATORY_FIELD,
        [Description("Can't validate an empty object")]
        CANT_VALIDATE_EMPTY_OBJECT,
        [Description("Amount is missing or zero")]
        AMOUNT_MISSING_OR_ZERO,
        [Description("Insufficient funds")]
        INSUFFICIENT_FUNDS,
        [Description("Cashier is not in the agent network")]
        CASHIER_IS_NOT_IN_THE_AGENT_NETWORK,
        [Description("User does not exist")]
        USER_DOES_NOT_EXIST,
        [Description("Agent does not exist")]
        AGENT_DOES_NOT_EXIST,
        [Description("User is not active")]
        USER_IS_NOT_ACTIVE,
        [Description("User account does not exist")]
        USER_ACCOUNT_NOT_EXIST,
        [Description("Money transfer cannot be refunded")]
        CANNOT_BE_REFUNDED,
        [Description("Money transfer does not exist")]
        MONEY_TRANSFER_DOES_NOT_EXIST,
        [Description("Invalid money transfer")]
        INVALID_MONEY_TRANSFER,
        [Description("Money transfer cannot be processed {0}")]
        MONEY_TRANSFER_CANNOT_BE_PROCESSED,
        [Description("Password reset cannot complete")]
        PASSWORD_RESET_CANNOT_COMPLETE,
        [Description("Cashier does not exist")]
        CASHIER_DOES_NOT_EXIST,
        [Description("Invalid device code")]
        INVALID_DEVICE_CODE,
        [Description("Device code has expired")]
        DEVICE_CODE_HAS_EXPIRED,
        [Description("Device token has expired")]
        DEVICE_TOKEN_HAS_EXPIRED,
        [Description("Device does not exist")]
        DEVICE_DOES_NOT_EXIST,
        [Description("Device not authorized")]
        DEVICE_NOT_AUTHORIZED,
        [Description("Invalid device token.{0}")]
        INVALID_DEVICE_TOKEN,
        [Description("Invalid cashier credentials")]
        INVALID_CASHIER_CREDENTIALS,
        [Description("Authorization failed")]
        AUTHORIZATION_FAILED,
        [Description("Invalid mobile number")]
        INVALID_MOBILE_NUMBER,
        [Description("Mobile number already in use")]
        MOBILE_NUMBER_ALREADY_IN_USE,
        [Description("Invalid email")]
        INVALID_EMAIL,
        [Description("Agent registration failed")]
        AGENT_REGISTRATION_FAILED
    }
}
