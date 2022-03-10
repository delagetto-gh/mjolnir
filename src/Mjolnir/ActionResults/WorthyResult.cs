using Microsoft.AspNetCore.Http;

namespace Mjolnir.ActionResults
{
    public class WorthyResult : CustomReasonResult
    {
        public WorthyResult()
        : base(StatusCodes.Status200OK, "Worthy")
        {
        }
    }
}
