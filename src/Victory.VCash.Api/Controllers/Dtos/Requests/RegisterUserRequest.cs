using FluentValidation;
using Victory.VCash.Infrastructure.Errors;

namespace Victory.VCash.Api.Controllers.Dtos.Requests
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
            RuleFor(x => x.CitizenId).NotEmpty().WithState(x => new Error(ErrorCode.MANDATORY_FIELD, nameof(x.CitizenId)));
            RuleFor(x => x.Email).NotEmpty().WithState(x => new Error(ErrorCode.MANDATORY_FIELD, nameof(x.Email)));
            RuleFor(x => x.EmailVerificationUrl).NotEmpty().WithState(x => new Error(ErrorCode.MANDATORY_FIELD, nameof(x.EmailVerificationUrl)));
            RuleFor(x => x.MobilePhoneNumber).NotEmpty().WithState(x => new Error(ErrorCode.MANDATORY_FIELD, nameof(x.MobilePhoneNumber)));
            RuleFor(x => x.ReceiveMarketingMessages).NotEmpty().WithState(x => new Error(ErrorCode.MANDATORY_FIELD, nameof(x.ReceiveMarketingMessages)));
            RuleFor(x => x.IsPoliticallyExposed).NotEmpty().WithState(x => new Error(ErrorCode.MANDATORY_FIELD, nameof(x.IsPoliticallyExposed)));
        }
    }
}
