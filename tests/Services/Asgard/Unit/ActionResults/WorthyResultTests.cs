using Asgard.ActionResults;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Unit.Utilities;
using Xunit;

namespace Unit.ActionResults
{
    public class WorthyResultTests
    {
        public class TheExecuteActionMethod : UnitTest
        {
            [Fact]
            public void ShouldSetTheHttpResponseStatusCodeTo200GivenActionContext()
            {
                var mockHttpResponse = new Mock<IHttpResponseFeature>();

                var actionContext = CreateActionContext(mockHttpResponse);

                var sut = Fixture.Create<WorthyResult>();

                sut.ExecuteResult(actionContext);

                mockHttpResponse.VerifySet(o => o.StatusCode = 200);
            }

            [Fact]
            public void ShouldSetTheHttpResponseReasonPhraseToWorthyGivenActionContext()
            {
                var mockHttpResponse = new Mock<IHttpResponseFeature>();

                var actionContext = CreateActionContext(mockHttpResponse);

                var sut = Fixture.Create<WorthyResult>();

                sut.ExecuteResult(actionContext);

                mockHttpResponse.VerifySet(o => o.ReasonPhrase = "Worthy");
            }

            private ActionContext CreateActionContext(Mock<IHttpResponseFeature> mockHttpResponse)
            {
                var mockFeatureCollection = new Mock<IFeatureCollection>();

                mockFeatureCollection
                .Setup(o => o.Get<IHttpResponseFeature>())
                .Returns(mockHttpResponse.Object);

                var httpContext = new DefaultHttpContext(mockFeatureCollection.Object);

                return new ActionContext
                {
                    HttpContext = httpContext
                };
            }
        }
    }
}
