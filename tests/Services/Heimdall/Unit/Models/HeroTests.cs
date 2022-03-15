using System;
using AutoFixture;
using FluentAssertions;
using Heimdall.Models;
using Unit.Utilities;
using Xunit;

namespace Unit.Models
{
    public class HeroTests
    {
        public class TheConstructor : UnitTest
        {
            [Theory]
            [InlineData("")]
            [InlineData(" ")]
            [InlineData(null)]
            [InlineData("   ")]
            public void ShouldThrowArgumentNullExceptionGivenEmptyName(string name)
            {
                var result = Record.Exception(() => new Hero(name));

                result.Should().BeOfType<ArgumentNullException>();
            }
        }

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