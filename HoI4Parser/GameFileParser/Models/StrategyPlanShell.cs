using HoI4Parser.Parsers;
using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoI4Parser.Models
{
    public class StrategyPlanShell : IParadoxRead
    {
        public IList<StrategyPlan> StrategyPlanList { get; set; }

        public StrategyPlanShell()
        {
            StrategyPlanList = new List<StrategyPlan>();
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            var id = token;
            StrategyPlan plan = parser.Parse(new StrategyPlan());
            plan.ID = id;

            if (plan.Ideology != null)
            {
                if(plan.Tag == null)
                {
                    plan.Tag = id.Substring(0, 3);
                }

                plan.Enabled = !CountryParser.DisablePlanList.Contains(id);

                // Exclude this duplicate
                if(plan.Name != "Latvian historical plan" && plan.ID == "LAT_historical")
                    StrategyPlanList.Add(plan);
            }
        }
    }
}
