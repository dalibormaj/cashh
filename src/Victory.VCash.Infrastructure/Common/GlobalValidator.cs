using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Victory.VCash.Infrastructure.Errors;

namespace Victory.VCash.Infrastructure.Common
{
    public class GlobalValidator : IGlobalValidator
    {
        private HttpContext httpContext;

        public GlobalValidator(IHttpContextAccessor context)
        {
            httpContext = context.HttpContext;
        }

        public ValidationResult Validate<T>(T instance)
        {
            if (instance == null)
                throw new VCashException(ErrorCode.CANT_VALIDATE_EMPTY_OBJECT);

            var validator = httpContext.RequestServices.GetService<IValidator<T>>();
            if (validator != null)
            {
                var validationResults = validator.Validate(instance);
                var errors = validationResults?.Errors
                                              ?.Where(x => x.CustomState is Error)
                                              ?.Select(x => (Error)x.CustomState)
                                              ?.ToList();

                if(errors?.Any() ?? false)
                    throw new VCashException(errors);

                //check for error messages that are not mapped with valid error code
                var errorMessages = validationResults?.Errors
                                                     ?.Where(x => x.CustomState is not ErrorCode)
                                                     ?.Select(x => x.ErrorMessage);

                var firstErrorMessage = errorMessages?.FirstOrDefault();
                if (!string.IsNullOrEmpty(firstErrorMessage))
                    throw new VCashException(firstErrorMessage);

                return validationResults;
            }

            return null;
        }
    }
}
