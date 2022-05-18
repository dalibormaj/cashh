using System;
using System.Collections.Generic;
using System.Linq;
using Victory.VCash.Infrastructure.Extensions;
using Victory.VCash.Infrastructure.Resources;

namespace Victory.VCash.Infrastructure.Errors
{
    public abstract class BaseException : Exception
    {
        public List<Error> Errors { get; private set; }

        public BaseException(List<Error> errors) : base(string.Join("; ", errors?.Select(x => string.Format(x.ErrorCode.GetDescription(tryTranslate: true), x.Args))))
        {
            Errors = errors;
        }

        public BaseException(ErrorCode errorCode, params string[] args) : this(new List<Error>() { new Error(errorCode, args)})
        {
        }

        public BaseException(string message) : base(ResourceManager.GetText(message))
        {
        }

        public BaseException(string message, params string[] args) : base(string.Format(ResourceManager.GetText(message), args))
        {
        }
    }

    public class Error
    {
        public ErrorCode ErrorCode { get; private set; }
        public string[] Args { get; private set; }

        public Error(ErrorCode errorCode, params string[] args)
        {
            //try translating args
            args = args.ToList().Select(x => ResourceManager.GetText(x)).ToArray();

            ErrorCode = errorCode;
            Args = args;
        }
    }
}
