using Microsoft.AspNetCore.Http;

namespace Mjolnir.Api.ActionResults
{
    public class UnworthyResult : CustomReasonResult
    {
        public UnworthyResult()
        : base(StatusCodes.Status403Forbidden, "Unworthy")
        {
        }
    }
}
