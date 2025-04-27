using Core;
using Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public  class WeightCategoryRanges:IWeightCategory
    {
        public readonly Dictionary<WeightCategory, (float MinBMI, float MaxBMI)> _BMIRanges = new()
    {
        { WeightCategory.Underweight, (0, 18.5f) },
        { WeightCategory.Normal, (18.5f, 24.9f) },
        { WeightCategory.Overweight, (25.0f, 29.9f) },
        { WeightCategory.Obese, (30.0f, float.MaxValue) }
    };

        public readonly Dictionary<WeightCategory, (float MinFatPercentage, float MaxFatPercentage)> _FatPercentageRanges = new()
    {
        { WeightCategory.Underweight, (0, 5) },
        { WeightCategory.Normal, (6, 20) },
        { WeightCategory.Overweight, (21, 30) },
        { WeightCategory.Obese, (31, float.MaxValue) }


    };

        public Dictionary<WeightCategory, (float MinBMI, float MaxBMI)> BMIRanges()
        {
            return _BMIRanges;
        }

        public Dictionary<WeightCategory, (float MinFatPercentage, float MaxFatPercentage)> FatPercentageRanges()
        {
            return _FatPercentageRanges;
        }

        public   WeightCategory GetUserWeightCategory(float bmi, float bodyFatPercentage)
        {
            foreach (var category in _BMIRanges.Keys)
            {
                var bmiRange = _BMIRanges[category];
                var fatRange = _FatPercentageRanges[category];

                if (bmi >= bmiRange.MinBMI && bmi <= bmiRange.MaxBMI &&
                    bodyFatPercentage >= fatRange.MinFatPercentage && bodyFatPercentage <= fatRange.MaxFatPercentage)
                {
                    return category;
                }
            }

            // Default to Normal if no match is found
            return WeightCategory.Normal;
        }
    }
    public  class WeightCategoryTargets:IWeightTarget
    {
        public readonly Dictionary<WeightCategory, (float TargetBMI, float TargetWeightChange, float TargetFatPercentageChange)> _Targets = new()
        {
            { WeightCategory.Underweight, (22.0f, 5.0f, 3.0f) }, // Gain weight and increase fat slightly
            { WeightCategory.Normal, (22.5f, 0.0f, 0.0f) },       // Maintain current metrics
            { WeightCategory.Overweight, (23.0f, -5.0f, -5.0f) }, // Lose weight and reduce fat
            { WeightCategory.Obese, (25.0f, -10.0f, -10.0f) }     // Significant weight and fat loss
        };

        public Dictionary<WeightCategory, (float TargetBMI, float TargetWeightChange, float TargetFatPercentageChange)> Targets()
        {
            return _Targets;
        }
        public (float TargetBMI, float TargetWeightChange, float TargetFatPercentageChange) GetTargetForCategory(WeightCategory category)
        {
            return _Targets[category];
        }
    }   
}
