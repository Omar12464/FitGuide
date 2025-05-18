
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core
{
    public class Exercise: ModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public FitnessLevel Difficulty { get; set; }
        public string? TypeOfMachine { get; set; }
        public string? TargetMuscle { get; set; }
        public List<string>? TargetInjury { get; set; }
        public string? YoutubeLink { get; set; }
        //public byte[] GifBytes { get; set; }
        //public string GifPath { get; set; }
        [JsonIgnore]
        public ICollection<WorkOutExercises> workOutExercises { get; set; } = new HashSet<WorkOutExercises>();

    }
}
