﻿using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class NutritionPlan : ModelBase
    {

        public string UserId { get; set; }
        public string Name { get; set; }
        public double CaloriestTarget { get; set; }
        public double ProteinTarget { get; set; }
        public double CarbsTarget { get; set; }
        public double FatTarget { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    }
}
