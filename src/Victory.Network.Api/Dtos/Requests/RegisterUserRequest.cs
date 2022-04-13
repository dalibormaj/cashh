using FluentValidation;
using Victory.Network.Infrastructure.Errors;

namespace Victory.Network.Api.Dtos.Requests
{
    public class RegisterUserRequest
    {
        public string CitizenId { get; set; }
        public string EmailVerificationUrl { get; set; }
        public string Email { get; set; }
        public string MobilePhoneNumber { get; set; } 
        public bool? ReceiveMarketingMessages { get; set; }
        public bool? IsPoliticallyExposed { get; set; }
    }

    public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserRequestValidator()
        {
            //Mandatory fields
            RuleFor(x => x.CitizenId).NotEmpty().WithState(x => ErrorCode.CITIZEN_ID_MISSING);
            RuleFor(x => x.Email).NotEmpty().WithState(x => ErrorCode.EMAIL_MISSING);
            RuleFor(x => x.EmailVerificationUrl).NotEmpty().WithState(x => ErrorCode.EMAIL_VERIFICATION_URL_MISSING);
            RuleFor(x => x.MobilePhoneNumber).NotEmpty().WithState(x => ErrorCode.MOBILE_NUMBER_MISSING);
            RuleFor(x => x.ReceiveMarketingMessages).NotEmpty().WithState(x => ErrorCode.RECEIVE_MARKETING_MESSAGES_MISSING);
            RuleFor(x => x.IsPoliticallyExposed).NotEmpty().WithState(x => ErrorCode.IS_POLITICALLY_EXPOSED_MISSING);
        }
    }
}
