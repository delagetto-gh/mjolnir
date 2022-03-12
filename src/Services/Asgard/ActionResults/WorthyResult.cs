using Microsoft.AspNetCore.Http;

namespace Asgard.ActionResults
{
    public class WorthyResult : CustomReasonResult
    {
        public WorthyResult()
        : base(StatusCodes.Status200OK, "Worthy")
        {
        }
    }
}
