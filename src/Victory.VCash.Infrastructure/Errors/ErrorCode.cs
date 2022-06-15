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
        [Description("PL User cannot be found")]
        PL_USER_CANNOT_BE_FOUND,
        [Description("Agent cannot be found {0}")]
        AGENT_CANNOT_BE_FOUND,
        [Description("PL User is not active {0}")]
        PL_USER_IS_NOT_ACTIVE,
        [Description("PL User account cannot be found")]
        PL_USER_ACCOUNT_CANNOT_BE_FOUND,
        [Description("Money transfer cannot be refunded")]
        CANNOT_BE_REFUNDED,
        [Description("Money transfer cannot be found")]
        MONEY_TRANSFER_CANNOT_BE_FOUND,
        [Description("Invalid money transfer")]
        INVALID_MONEY_TRANSFER,
        [Description("Money transfer cannot be processed {0}")]
        MONEY_TRANSFER_CANNOT_BE_PROCESSED,
        [Description("Password reset cannot complete")]
        PASSWORD_RESET_CANNOT_COMPLETE,
        [Description("Password does not meet complexity requirements")]
        PASSWORD_DOES_NOT_MEET_COMPLEXITY_REQUIREMENTS,
        [Description("Cashier cannot be found")]
        CASHIER_CANNOT_BE_FOUND,
        [Description("Cashier already registered")]
        CASHIER_ALREADY_REGISTERED,
        [Description("Invalid device code")]
        INVALID_DEVICE_CODE,
        [Description("Device code has expired")]
        DEVICE_CODE_HAS_EXPIRED,
        [Description("Device token has expired")]
        DEVICE_TOKEN_HAS_EXPIRED,
        [Description("Device cannot be found")]
        DEVICE_CANNOT_BE_FOUND,
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
        AGENT_REGISTRATION_FAILED,
        [Description("Parent agent cannot be found")]
        PARENT_AGENT_CANNOT_BE_FOUND,
        [Description("Company cannot be found")]
        COMPANY_CANNOT_BE_FOUND,
        [Description("More companies already registed with the same tax number")]
        MORE_COMPANIES_ALREADY_REGISTED_WITH_THE_SAME_TAX_NUMBER,
        [Description("Agent is not active")]
        AGENT_IS_NOT_ACTIVE,
        [Description("Parent agent is not active")]
        PARENT_AGENT_IS_NOT_ACTIVE,
        [Description("Agent already exists")]
        AGENT_ALREADY_EXISTS,
        [Description("Venue does not belong to the agent")]
        VENUE_DOES_NOT_BELONG_TO_THE_AGENT,
        [Description("Venue cannot be found")]
        VENUE_CANNOT_BE_FOUND,
        [Description("Agent is not in status pending verification")]
        AGENT_IS_NOT_IN_STATUS_PENDING_VERIFICATION,
        [Description("Invalid verification code")]
        INVALID_VERIFICATION_CODE
    }
}
