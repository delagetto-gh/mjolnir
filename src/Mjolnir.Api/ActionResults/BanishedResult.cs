using Microsoft.AspNetCore.Http;

namespace Mjolnir.Api.ActionResults
{
    public class BanishedResult : CustomReasonResult
    {
        public BanishedResult()
        : base(StatusCodes.Status401Unauthorized, "Banished")
        {
        }
    }
}
