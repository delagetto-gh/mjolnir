using System;

namespace Heimdall.Exceptions
{
    public class HeroNameTakenException : Exception
    {
        public HeroNameTakenException()
        {
        }

        public HeroNameTakenException(string message) : base(message)
        {
        }
    }
}