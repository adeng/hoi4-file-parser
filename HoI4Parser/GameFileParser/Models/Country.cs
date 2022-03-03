using System;
using System.Collections.Generic;
using System.Text;

namespace HoI4Parser.Models
{
    public class Country
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Color { get; set; }
        
        public Country(string name, string tag, string color)
        {
            Name = name;
            Tag = tag;
            Color = color;
        }
    }
}
