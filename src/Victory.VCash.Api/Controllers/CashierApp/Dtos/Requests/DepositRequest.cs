using FluentValidation;
using System.ComponentModel.DataAnnotations;
using Victory.VCash.Infrastructure.Errors;

namespace Victory.VCash.Api.Controllers.CashierApp.Dtos.Requests
{
    public class DepositRequest
    {
        [Required]
        public int ToUserId { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }

    public class DepositRequestValidator : AbstractValidator<DepositRequest>
    {
        public DepositRequestValidator()
        {
            //Mandatory fields
            RuleFor(x => x.ToUserId).NotEmpty().WithState(x => new Error(ErrorCode.MANDATORY_FIELD, nameof(x.ToUserId)));
            RuleFor(x => x.Amount).NotEmpty().WithState(x => new Error(ErrorCode.MANDATORY_FIELD, nameof(x.Amount)));
        }
    }
}
