using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public interface ILogFoodService
    {
        public Task LogFood(string userId,int foodItemId,double quantity);
        public bool IsFoodSafeForUser(FoodItem food, List<string> userAllergies);

    
    }
}
