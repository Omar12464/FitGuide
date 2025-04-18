using Core;
using Core.Interface;
using Core.Interface.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class UserMetrisService : IUserMetricsServices
    {
        private readonly FitGuideContext _context;

        public UserMetrisService(FitGuideContext context)
        {
            _context = context;
        }

        public UserMetrisService()
        {
            
        }

        public float CalculateBMI(float weight, float Height)
        {
            var heightinCM = Height / 100.0f;
            var bmi = weight / (heightinCM * heightinCM);
            return (float)Math.Round(bmi,2);
        }
        public WeightCategory GetWeightCategory(float bmi)
        {

            if (bmi < 18.5)
            {
                return WeightCategory.Underweight;               
            }
            else if (bmi >= 18.5 && bmi < 24.9)
            {
                return WeightCategory.Normal;
            }
            else if (bmi >= 25 && bmi < 29.9)
            {
                return WeightCategory.Overweight;
            }
            else
            {
                return WeightCategory.Obese;
            }
        }

        public async Task<bool> CheckMetrics(string userid)
        {
            return await _context.userMetrics.AnyAsync(ua => ua.UserId == userid);
        }
    }
}
