using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DogGo.Models
{
    public class Walker
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public int NeighborhoodId { get; set; }
        [MaxLength(255)]
        public string ImageUrl { get; set; }
        [Required]
        public Neighborhood Neighborhood { get; set; }
        public List<Walk> Walks { get; set; }
    }
}