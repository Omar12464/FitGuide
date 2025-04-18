using Core;
using Core.Interface;
using Core.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class UserMetrisService : IUserMetricsServices
    {
        public float CalculateBMI(float weight, float Height)
        {
            var heightinCM = Height / 100.0f;
            var bmi = weight / (heightinCM * heightinCM);
            return (float)Math.Round(bmi,2);
        }
    }
}
