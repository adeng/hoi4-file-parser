using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoI4Parser.Models
{
    public class TerrainShell : IParadoxRead
    {
        public IList<Terrain> Terrains { get; set; }

        public TerrainShell()
        {
            Terrains = new List<Terrain>();
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            var id = token;
            Terrains.Add(parser.Parse(new Terrain()));
            Terrains[Terrains.Count - 1].ID = id;
        }
    }
}
