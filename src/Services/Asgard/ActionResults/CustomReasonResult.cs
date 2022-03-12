using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace Asgard.ActionResults
{
    public abstract class CustomReasonResult : ActionResult
    {
        private readonly int _statusCode;
        private readonly string _reasonPhrase;

        internal CustomReasonResult(int statusCode, string reasonPhrase)
        {
            _statusCode = statusCode;
            _reasonPhrase = reasonPhrase;
        }
        public override void ExecuteResult(ActionContext context)
        {
            base.ExecuteResult(context);
            var response = context?.HttpContext?.Features?.Get<IHttpResponseFeature>();
            if (response != null)
            {
                response.StatusCode = _statusCode;
                response.ReasonPhrase = _reasonPhrase;
            }
        }
    }
}
