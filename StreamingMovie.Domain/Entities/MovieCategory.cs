using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Domain.Entities
{
    /// <summary>
    /// Entity class for movie_category table
    /// </summary>
    public class MovieCategory
    {
        public int MovieId { get; set; }

        public int CategoryId { get; set; }

        // Navigation Properties
        public Movie Movie { get; set; }
        public Category Category { get; set; }
    }
}
