﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Models
{
    public class MovieRequest
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Range(1888, int.MaxValue, ErrorMessage = "Incorrect Year!")]
        public int Year { get; set; }
    }
}
