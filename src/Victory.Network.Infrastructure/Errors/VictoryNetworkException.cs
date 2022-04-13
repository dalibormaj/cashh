using System.Collections.Generic;

namespace Victory.Network.Infrastructure.Errors
{
    public class VictoryNetworkException : BaseException
    {
        public VictoryNetworkException(List<ErrorCode> errorCodes) : base(errorCodes)
        {
        }

        public VictoryNetworkException(ErrorCode errorCode) : base(errorCode)
        {
        }

        public VictoryNetworkException(string message) : base(message)
        {
        }
    }
}
