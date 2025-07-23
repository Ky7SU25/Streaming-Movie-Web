using StreamingMovie.Domain.Interfaces;

namespace StreamingMovie.Domain.UnitOfWorks
{
    /// <summary>
    /// Interface for UnitOfWork
    /// </summary>
    public interface IUnitOfWork
    {
        IActorRepository ActorRepository { get; }
        IBannerRepository BannerRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICommentRepository CommentRepository { get; }
        ICommentReactionRepository CommentReactionRepository { get; }
        ICountryRepository CountryRepository { get; }
        IDirectorRepository DirectorRepository { get; }
        IEpisodeRepository EpisodeRepository { get; }
        IEpisodeVideoRepository EpisodeVideoRepository { get; }
        IFavoriteRepository FavoriteRepository { get; }
        IMovieRepository MovieRepository { get; }
        IMovieActorRepository MovieActorRepository { get; }
        IMovieCategoryRepository MovieCategoryRepository { get; }
        IMovieDirectorRepository MovieDirectorRepository { get; }
        IMovieVideoRepository MovieVideoRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IPlaylistRepository PlaylistRepository { get; }
        IPlaylistItemRepository PlaylistItemRepository { get; }
        IRatingRepository RatingRepository { get; }
        IRoleRepository RoleRepository { get; }
        ISeriesRepository SeriesRepository { get; }
        ISeriesActorRepository SeriesActorRepository { get; }
        ISeriesCategoryRepository SeriesCategoryRepository { get; }
        ISeriesDirectorRepository SeriesDirectorRepository { get; }
        IUserRepository UserRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        IUserTokenRepository UserTokenRepository { get; }
        IVideoQualityRepository VideoQualityRepository { get; }
        IVideoServerRepository VideoServerRepository { get; }
        IWatchHistoryRepository WatchHistoryRepository { get; }
        IUnifiedMovieRepository UnifiedMovieRepository { get; }
        IPaymentRepository PaymentRepository { get; }
    }
}
