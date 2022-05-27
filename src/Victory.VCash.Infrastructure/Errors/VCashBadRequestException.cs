using System.Collections.Generic;

namespace Victory.VCash.Infrastructure.Errors
{
    public class VCashBadRequestException : BaseException
    {
        public VCashBadRequestException(List<Error> errors) : base(errors)
        {
        }

        public VCashBadRequestException(ErrorCode errorCode, params string[] args) : base(errorCode, args)
        {
        }

        /// <summary>
        /// Translatable Victory network exception
        /// </summary>
        /// <param name="message">Error message. It can contain format items like {0}{1} that will be replaced with list of args</param>
        /// <param name="args">List of arguments that will be used while forming the error message</param>
        public VCashBadRequestException(string message, params string[] args) : base(message, args)
        {
        }
    }
}
