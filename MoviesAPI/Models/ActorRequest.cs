using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Models
{
    public class ActorRequest
    {

        [Key]
        public int ActorId { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Bad Length")]
        public string Name { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Bad Length")]
        public string Surname { get; set; }
        [Range(1888, int.MaxValue, ErrorMessage = "Incorrect Year!")]
        public int YearOfBirth { get; set; }
    }
}
