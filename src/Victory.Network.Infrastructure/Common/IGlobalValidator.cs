using FluentValidation.Results;

namespace Victory.Network.Infrastructure.Common
{
    public interface IGlobalValidator
    {
        ValidationResult Validate<T>(T instance);
    }
}
