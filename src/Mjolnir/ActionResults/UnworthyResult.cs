using Microsoft.AspNetCore.Http;

namespace Mjolnir.ActionResults
{
    public class UnworthyResult : CustomReasonResult
    {
        public UnworthyResult()
        : base(StatusCodes.Status403Forbidden, "Unworthy")
        {
        }
    }
}
