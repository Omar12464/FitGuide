using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class DailyIntake:ModelBase
    {
        public string UserId { get; set; } // Foreign key to the user
        public DateTime Date { get; set; } // Date of the intake

        // Aggregated nutritional values
        public double TotalCalories { get; set; }
        public double TotalProtein { get; set; }
        public double TotalCarbs { get; set; }
        public double TotalFat { get; set; }
        //public User user { get; set; }
    }
}
