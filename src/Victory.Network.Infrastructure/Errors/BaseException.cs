using System;
using System.Collections.Generic;
using System.Linq;
using Victory.Network.Infrastructure.Extensions;
using Victory.Network.Infrastructure.Resources;

namespace Victory.Network.Infrastructure.Errors
{
    public abstract class BaseException : Exception
    {
        public List<ErrorCode> ErrorCodes { get; private set; }
        
        public BaseException(List<ErrorCode> errorCodes) : base(string.Join("; ", errorCodes?.Select(x => x.GetDescription(tryTranslate: true))))
        {
            ErrorCodes = errorCodes;
        }

        public BaseException(ErrorCode errorCode) : this(new List<ErrorCode>() { errorCode })
        {
        }

        public BaseException(string message) : base(ResourceManager.GetText(message))
        {
        }
    }
}
