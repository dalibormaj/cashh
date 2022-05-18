using Victory.VCash.Infrastructure.Errors;

namespace Victory.VCash.Api.Controllers.Dtos
{
    public class ErrorDto
    {
        public ErrorCode? Code { get; set; }
        public string Description { get; set; }
    }
}
