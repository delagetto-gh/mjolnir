using System;

namespace Heimdall.Exceptions
{
    public class HeroNameTakenException : Exception
    {
        public HeroNameTakenException() : this(string.Empty)
        {
        }

        public HeroNameTakenException(string heroName) : base($"Hero name is already taken. {heroName}")
        {
        }
    }
}