using Microsoft.AspNetCore.Http;

namespace Mjolnir.Api.ActionResults
{
    public class WorthyResult : CustomReasonResult
    {
        public WorthyResult()
        : base(StatusCodes.Status200OK, "Worthy")
        {
        }
    }
}
