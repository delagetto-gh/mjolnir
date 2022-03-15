using AutoFixture;
using AutoFixture.AutoMoq;

namespace Unit.Utilities
{
    public abstract class UnitTest
    {
        public UnitTest()
        {
            Fixture = new Fixture().Customize(new AutoMoqCustomization());
        }
        
        public IFixture Fixture { get; }
    }
}