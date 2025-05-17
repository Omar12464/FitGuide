using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{

    public class FoodItem
    {
        // Primary Key
        public int Id { get; set; }

        // General Information
        public string Name { get; set; } // Name of the food
        public string Description { get; set; } // Brief description of the food
        public string Category { get; set; } // Category: Fruits, Vegetables, Grains, Proteins, Dairy, etc.

        // Nutritional Data (per 100g serving)
        public double CaloriesPerServing { get; set; } // Calories per 100g
        public double ProteinPerServing { get; set; } // Protein content per 100g (in grams)
        public double CarbsPerServing { get; set; } // Carbohydrate content per 100g (in grams)
        public double FatPerServing { get; set; } // Fat content per 100g (in grams)
        public int ServingSize { get; set; } = 100; // Fixed at "100g"

        // Dietary Flags
        public bool IsVegetarian { get; set; } // Vegetarian-friendly
        public bool IsVegan { get; set; } // Vegan-friendly
        public bool IsGlutenFree { get; set; } // Gluten-free
        public bool IsNutFree { get; set; } // Nut-free
        public bool IsDairyFree { get; set; } // Dairy-free

        // Allergen Flags
        public bool IsPeanutFree { get; set; } // Peanut-free
        public bool IsShellfishFree { get; set; } // Shellfish-free
        public bool IsFishFree { get; set; } // Fish-free
        public bool IsSoyFree { get; set; } // Soy-free
        public bool IsWheatFree { get; set; } // Wheat-free
        public bool IsSesameFree { get; set; } // Sesame-free
        public bool IsMustardFree { get; set; } // Mustard-free
        public bool IsCeleryFree { get; set; } // Celery-free
        public bool IsLupinFree { get; set; } // Lupin-free
        public bool IsMolluscsFree { get; set; } // Molluscs-free
        public bool IsSulfiteFree { get; set; } // Sulfite-free
        public bool IsFruitFree { get; set; } // Free from fruit allergens
        public bool IsLegumeFree { get; set; } // Legume-free

        // Additional Dietary Preferences
        public bool IsLowFODMAP { get; set; } // Low FODMAP diet compatibility
        public bool IsKosher { get; set; } // Kosher-certified
        public ICollection<LogFood> logFoods { get; set; }=new HashSet<LogFood>(); // Collection of food logs associated with this food item
    }
    //public string? BarCode { get; set; }

}

