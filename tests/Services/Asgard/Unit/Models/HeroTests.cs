using Asgard.Models;
using AutoFixture;
using FluentAssertions;
using Unit.Utilities;
using Xunit;

namespace Unit.Models
{
    public class HeroTests
    {
        public class TheNameProperty : UnitTest
        {
            [Fact]
            public void ShouldReturnCorrectName()
            {
                var name = Fixture.Create<string>();

                var sut = new Hero(name);

                sut.Name.Should().Be(name);
            }
        }
    }
}