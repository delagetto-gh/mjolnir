using Asgard.ActionResults;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Unit.ActionResults
{
    public class UnworthyResultTests
    {
        public class TheExecuteActionMethod
        {
            [Fact]
            public void ShouldSetTheHttpResponseStatusCodeTo403GivenActionContext()
            {
                // Given
                var actionContext = CreateActionContext();
                var sut = new UnworthyResult();

                // When
                sut.ExecuteResult(actionContext);

                // Then
                var result = actionContext.HttpContext.Features.Get<IHttpResponseFeature>();
                result.StatusCode.Should().Be(403);
            }

            [Fact]
            public void ShouldSetTheHttpResponseReasonPhraseToWorthyGivenActionContext()
            {
                // Given
                var actionContext = CreateActionContext();
                var sut = new UnworthyResult();

                // When
                sut.ExecuteResult(actionContext);

                // Then
                var result = actionContext.HttpContext.Features.Get<IHttpResponseFeature>();
                result.ReasonPhrase.Should().Be("Unworthy");
            }

            private static ActionContext CreateActionContext()
            {
                var httpResponseFeature = new HttpResponseFeature();
                var featureCollection = new FeatureCollection();
                featureCollection.Set<IHttpResponseFeature>(httpResponseFeature);

                var httpContext = new DefaultHttpContext(featureCollection);

                var actionContext = new ActionContext();
                actionContext.HttpContext = httpContext;
                return actionContext;
            }
        }
    }
}