using IronDomeAPI.Enums;
using System.ComponentModel.DataAnnotations;
namespace IronDomeAPI.Models
{
    public class Attack
    {
        public Guid? id {  get; set; }
        [AllowedValues("Iran","Hutim")]
        public string? origin { get; set; }
        public string? type { get; set; }

        public attackStatuses status { get; set; }
        public DateTime startedAt { get; set; }
        public int? missileCount { get; set; }
        public List<string>? missileTypes  { get; set; }
    }
}
