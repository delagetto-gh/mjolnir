using Microsoft.AspNetCore.Http;

namespace Asgard.ActionResults
{
    public class UnworthyResult : CustomReasonResult
    {
        public UnworthyResult()
        : base(StatusCodes.Status403Forbidden, "Unworthy")
        {
        }
    }
}
