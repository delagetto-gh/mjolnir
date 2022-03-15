using System;

namespace Heimdall.Models
{
    public class Hero
    {
        public Hero(string heroName)
        {
            Name = !string.IsNullOrWhiteSpace(heroName) ? 
                   heroName : throw new ArgumentNullException();
        }
        public string Name { get; }
    }
}