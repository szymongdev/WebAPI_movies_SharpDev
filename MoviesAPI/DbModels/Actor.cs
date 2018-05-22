using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DbModels
{
    public class Actor
    {
        [Key]
        public int ActorId { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Incorrect Length!")]
        public string Name { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Incorrect Length!")]
        public string Surname { get; set; }

        [Range(1888, int.MaxValue, ErrorMessage = "Incorrect Year!")]
        public int YearOfBirth { get; set; }

        public virtual ICollection<MovieActor> MovieActors { get; set; }
    }
}
