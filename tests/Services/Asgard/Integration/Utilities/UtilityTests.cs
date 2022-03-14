using Xunit;

namespace Integration.Utilities
{
    public class UtilityTests
    {
        [Fact]
        internal void QuicklyGenerateJwt()
        {
            var secret = "secretGoesHere1234";
            var heroName = "heroNameGoesHere12334";

            var gen = new AsgardPassGenerator(secret);

            var jwt = gen.Generate(heroName);
        }
    }
}