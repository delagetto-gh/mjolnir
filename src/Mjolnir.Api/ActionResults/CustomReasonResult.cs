using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace Mjolnir.Api.ActionResults
{
    public abstract class CustomReasonResult : StatusCodeResult
    {
        private readonly string _reasonPhrase;

        internal CustomReasonResult(int statusCode, string reasonPhrase)
        : base(statusCode)
        {
            _reasonPhrase = reasonPhrase;
        }
        public override void ExecuteResult(ActionContext context)
        {
            base.ExecuteResult(context);
            var response = context?.HttpContext?.Features?.Get<IHttpResponseFeature>();
            if (response != null)
                response.ReasonPhrase = _reasonPhrase;
        }
    }
}
