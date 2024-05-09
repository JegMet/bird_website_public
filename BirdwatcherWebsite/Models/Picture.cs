using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BirdwatcherWebsite.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        [Display(Name = "Date Image Taken")]
        [DataType(DataType.Date)]
        public DateTime DateTimeImgTaken { get; set; }
        public string? BirdType { get; set; }
    }
}
