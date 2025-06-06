using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Domain.Entities
{
    /// <summary>
    /// Entity class for series_category table
    /// </summary>
    public class SeriesCategory
    {
        public int SeriesId { get; set; }

        public int CategoryId { get; set; }

        // Navigation Properties
        public Series Series { get; set; }
        public Category Category { get; set; }
    }
}
