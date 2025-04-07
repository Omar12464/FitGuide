using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class WorkOutPlan
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfDays { get; set; }
        //public DateOnly StartDate { get; set; }
        //public DateOnly EndDate { get; set; }
        public string UserId { get; set; }

    }
}
