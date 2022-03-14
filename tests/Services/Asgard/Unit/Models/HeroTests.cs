using Asgard.Models;
using FluentAssertions;
using Xunit;

namespace Unit.Models
{
    public class HeroTests
    {
        public class TheNameProperty
        {
            [Theory]
            [InlineData("Thot")]
            [InlineData("Captain Underpants")]
            [InlineData("Black Rabbit")]
            [InlineData("Four Loko")]
            [InlineData("Sight")]
            [InlineData("Soupman")]
            public void ShouldReturnCorrectName(string name)
            {
                var sut = new Hero(name);

                sut.Name.Should().Be(name);
            }
        }
    }
}