using Victory.Network.Infrastructure.Errors;

namespace Victory.Network.Api.Dtos.Responses
{
    public class ErrorResponse
    {
        public ErrorCode? Code { get; set; }
        public string Description { get; set; }
    }
}
