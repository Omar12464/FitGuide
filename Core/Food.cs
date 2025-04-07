using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Food
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public float Calories { get; set; }
        public float Carbs { get; set; }
        public float Protein { get; set; }
        public float Fats { get; set; }
        public float ServingSize { get; set; } = 100;
        public string? BarCode { get; set; }

    }
}
