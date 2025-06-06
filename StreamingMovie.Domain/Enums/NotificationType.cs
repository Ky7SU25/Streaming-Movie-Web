using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingMovie.Domain.Enums;

/// <summary>
/// The type of a notification
/// </summary>
public enum NotificationType
{
    NewMovie,
    NewEpisode,
    System,
    Promotion
}
