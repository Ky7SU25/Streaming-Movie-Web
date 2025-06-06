using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StreamingMovie.Domain.Entities
{
    /// <summary>
    /// Entity class for movie_actor table
    /// </summary>
    public class MovieActor
    {
        public int MovieId { get; set; }

        public int ActorId { get; set; }

        [MaxLength(100)]
        public string? CharacterName { get; set; }

        public bool? IsMainCharacter { get; set; } = false;

        // Navigation Properties
        public Movie Movie { get; set; }
        public Actor Actor { get; set; }
    }
}
