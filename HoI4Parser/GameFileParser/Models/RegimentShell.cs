using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoI4Parser.Models
{
    public class RegimentShell : IParadoxRead
    {
        public IList<Regiment> StatisticsList { get; set; }

        public RegimentShell()
        {
            StatisticsList = new List<Regiment>();
        }


        public void TokenCallback(ParadoxParser parser, string token)
        {
            var id = token;
            StatisticsList.Add(parser.Parse(new Regiment()));
            StatisticsList[StatisticsList.Count - 1].ID = id;
        }
    }
}
