using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoI4Parser.Models
{
    public class RegimentFamily : IParadoxRead
    {
        public IList<Regiment> RegimentList { get; set; }

        public RegimentFamily()
        {
            RegimentList = new List<Regiment>();
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            RegimentList = parser.Parse(new RegimentShell()).StatisticsList;
        }
    }
}
