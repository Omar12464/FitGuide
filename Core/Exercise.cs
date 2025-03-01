using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }
        public string TypeOfMachine { get; set; }
        public string TargetMuscle { get; set; }
        public byte[] GifBytes { get; set; }
        public string GifPath { get; set; }


    }
}
