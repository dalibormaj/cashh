using FluentValidation;
using System.ComponentModel.DataAnnotations;
using Victory.VCash.Infrastructure.Errors;

namespace Victory.VCash.Api.Controllers.CashierApp.Dtos.Requests
{
    public class PayoutRequest
    {
        [Required]
        public int FromUserId { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }

    public class PayoutRequestValidator : AbstractValidator<PayoutRequest>
    {
        public PayoutRequestValidator()
        {
            //Mandatory fields
            RuleFor(x => x.FromUserId).NotEmpty().WithState(x => new Error(ErrorCode.MANDATORY_FIELD, nameof(x.FromUserId)));
            RuleFor(x => x.Amount).NotEmpty().WithState(x => new Error(ErrorCode.MANDATORY_FIELD, nameof(x.Amount)));
        }
    }
}
