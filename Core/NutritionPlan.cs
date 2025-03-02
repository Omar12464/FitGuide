using Core.Identity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class NutritionPlan
    {
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public User user { get; set; }
        public string Name { get; set; }
        public float CaloriestTarget { get; set; }
        public float ProteinTarget { get; set; }
        public float CarbsTarget { get; set; }
        public float FatTarget { get; set; }
        public int MyProperty { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    }
}
