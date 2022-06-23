using System.ComponentModel.DataAnnotations;

namespace DogGo.Models
{
    public class Dog
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public Owner Owner { get; set; }
        [Required]
        [MaxLength(30)]
        public string Breed { get; set; }
        [MaxLength(255)]
        public string Notes { get; set; }
        [MaxLength(255)]
        public string ImageUrl { get; set; }
    }
}
