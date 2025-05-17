using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class LogFood:ModelBase
    {
        public string UserId { get; set; }
        public int FoodId { get; set; }
        public int MyProperty { get; set; }
        public double Quantity { get; set; }
        public DateTime LoggedAt { get; set; }
        public FoodItem foodItem{ get; set; }
    }
}
