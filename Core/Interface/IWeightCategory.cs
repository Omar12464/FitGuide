using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IWeightCategory
    {
         Dictionary<WeightCategory, (float MinBMI, float MaxBMI)> BMIRanges();
         Dictionary<WeightCategory, (float MinFatPercentage, float MaxFatPercentage)> FatPercentageRanges();


        public  WeightCategory GetUserWeightCategory(float bmi, float bodyFatPercentage);
    }
}
