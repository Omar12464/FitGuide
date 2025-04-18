using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Services
{
    public interface IUserMetricsServices
    {
        public Task<bool> CheckMetrics(string userid);

        public float CalculateBMI(float weight, float Height);
        public WeightCategory GetWeightCategory(float bmi);

    }
}
