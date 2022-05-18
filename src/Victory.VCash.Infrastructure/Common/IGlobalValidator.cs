using FluentValidation.Results;

namespace Victory.VCash.Infrastructure.Common
{
    public interface IGlobalValidator
    {
        ValidationResult Validate<T>(T instance);
    }
}
