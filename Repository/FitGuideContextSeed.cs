using Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Repository
{
    public class FitGuideContextSeed
    {
        public static async Task SeedAsync(FitGuideContext fitGuideContext)
        {
            //if (await fitGuideContext.goalTempelates.AnyAsync()) return; // Skip if already seeded
            var options = new JsonSerializerOptions
            {
                MaxDepth = 64
            };
            var goalsData = File.ReadAllText("../Repository/JSONfiles/Goals.json");
            var goals = JsonSerializer.Deserialize<List<GoalTempelate>>(goalsData, options);
            if (goalsData.Count() > 0)
            {
                if (fitGuideContext.GoalTempelate.Count() == 0)
                {
                    foreach (var goal in goals)
                    {
                        fitGuideContext.Set<GoalTempelate>().Add(goal);
                    }
                    await fitGuideContext.SaveChangesAsync();

                }
            }
            var injurydata = File.ReadAllText("../Repository/JSONfiles/Injury.json");
            var injuries = JsonSerializer.Deserialize<List<Injury>>(injurydata, options);
            if(injurydata.Count() > 0)
            {
                if(fitGuideContext.Injury.Count()== 0)
                {
                    foreach(var injury in injuries)
                    {
                        fitGuideContext.Set<Injury>().Add(injury);
                    }
                    await fitGuideContext.SaveChangesAsync();
                }
            }
        }
    }
}
