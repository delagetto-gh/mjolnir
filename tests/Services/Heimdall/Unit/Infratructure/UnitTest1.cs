using System;
using Xunit;

namespace Unit.Infratructure
{
    public class UnitTest1
    {
        public class TheCreateHeroMethod : UnitTest
        {
            [Fact]
            public void ShouldReturnHeroGivenNoHeroAlreadyExistsWithSameName()
            {

                var sut = new HeroesManager();
                var result = sut.CreateHero(name, passowrd);
                result.Should().NotBeNull();
            }
        }
    }
}
