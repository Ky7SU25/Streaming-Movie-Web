using StreamingMovie.Domain.Interfaces;
using StreamingMovie.Domain.UnitOfWorks;
using StreamingMovie.Infrastructure.Data;
using StreamingMovie.Infrastructure.Repositories;

namespace StreamingMovie.Infrastructure.UnitOfWork
{
    /// <summary>
    /// MySQLUnitOfWork
    /// </summary>
    public class MySQLUnitOfWork : IUnitOfWork
    {
        private MovieDbContext _context;
        private IActorRepository _getActorRepository;
        private IBannerRepository _getBannerRepository;
        private ICategoryRepository _getCategoryRepository;
        private ICommentRepository _getCommentRepository;
        private ICommentReactionRepository _getCommentReactionRepository;
        private ICountryRepository _getCountryRepository;
        private IDirectorRepository _getDirectorRepository;
        private IEpisodeRepository _getEpisodeRepository;
        private IEpisodeVideoRepository _getEpisodeVideoRepository;
        private IFavoriteRepository _getFavoriteRepository;
        private IMovieRepository _getMovieRepository;
        private IMovieActorRepository _getMovieActorRepository;
        private IMovieCategoryRepository _getMovieCategoryRepository;
        private IMovieDirectorRepository _getMovieDirectorRepository;
        private IMovieVideoRepository _getMovieVideoRepository;
        private INotificationRepository _getNotificationRepository;
        private IPlaylistRepository _getPlaylistRepository;
        private IPlaylistItemRepository _getPlaylistItemRepository;
        private IRatingRepository _getRatingRepository;
        private IRoleRepository _getRoleRepository;
        private ISeriesRepository _getSeriesRepository;
        private ISeriesActorRepository _getSeriesActorRepository;
        private ISeriesCategoryRepository _getSeriesCategoryRepository;
        private ISeriesDirectorRepository _getSeriesDirectorRepository;
        private IUserRepository _getUserRepository;
        private IUserRoleRepository _getUserRoleRepository;
        private IUserTokenRepository _getUserTokenRepository;
        private IVideoQualityRepository _getVideoQualityRepository;
        private IVideoServerRepository _getVideoServerRepository;
        private IWatchHistoryRepository _getWatchHistoryRepository;
        private IUnifiedMovieRepository _getUnifiedMovieRepository;

        public MySQLUnitOfWork(MovieDbContext context)
        {
            _context = context;
        }

        public IActorRepository ActorRepository
        {
            get
            {
                return _getActorRepository ?? (_getActorRepository = new ActorRepository(_context));
            }
        }

        public IBannerRepository BannerRepository
        {
            get
            {
                return _getBannerRepository
                    ?? (_getBannerRepository = new BannerRepository(_context));
            }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                return _getCategoryRepository
                    ?? (_getCategoryRepository = new CategoryRepository(_context));
            }
        }

        public ICommentRepository CommentRepository
        {
            get
            {
                return _getCommentRepository
                    ?? (_getCommentRepository = new CommentRepository(_context));
            }
        }

        public ICommentReactionRepository CommentReactionRepository
        {
            get
            {
                return _getCommentReactionRepository
                    ?? (_getCommentReactionRepository = new CommentReactionRepository(_context));
            }
        }

        public ICountryRepository CountryRepository
        {
            get
            {
                return _getCountryRepository
                    ?? (_getCountryRepository = new CountryRepository(_context));
            }
        }

        public IDirectorRepository DirectorRepository
        {
            get
            {
                return _getDirectorRepository
                    ?? (_getDirectorRepository = new DirectorRepository(_context));
            }
        }

        public IEpisodeRepository EpisodeRepository
        {
            get
            {
                return _getEpisodeRepository
                    ?? (_getEpisodeRepository = new EpisodeRepository(_context));
            }
        }

        public IEpisodeVideoRepository EpisodeVideoRepository
        {
            get
            {
                return _getEpisodeVideoRepository
                    ?? (_getEpisodeVideoRepository = new EpisodeVideoRepository(_context));
            }
        }

        public IFavoriteRepository FavoriteRepository
        {
            get
            {
                return _getFavoriteRepository
                    ?? (_getFavoriteRepository = new FavoriteRepository(_context));
            }
        }

        public IMovieRepository MovieRepository
        {
            get
            {
                return _getMovieRepository ?? (_getMovieRepository = new MovieRepository(_context));
            }
        }

        public IMovieActorRepository MovieActorRepository
        {
            get
            {
                return _getMovieActorRepository
                    ?? (_getMovieActorRepository = new MovieActorRepository(_context));
            }
        }

        public IMovieCategoryRepository MovieCategoryRepository
        {
            get
            {
                return _getMovieCategoryRepository
                    ?? (_getMovieCategoryRepository = new MovieCategoryRepository(_context));
            }
        }

        public IMovieDirectorRepository MovieDirectorRepository
        {
            get
            {
                return _getMovieDirectorRepository
                    ?? (_getMovieDirectorRepository = new MovieDirectorRepository(_context));
            }
        }

        public IMovieVideoRepository MovieVideoRepository
        {
            get
            {
                return _getMovieVideoRepository
                    ?? (_getMovieVideoRepository = new MovieVideoRepository(_context));
            }
        }

        public INotificationRepository NotificationRepository
        {
            get
            {
                return _getNotificationRepository
                    ?? (_getNotificationRepository = new NotificationRepository(_context));
            }
        }

        public IPlaylistRepository PlaylistRepository
        {
            get
            {
                return _getPlaylistRepository
                    ?? (_getPlaylistRepository = new PlaylistRepository(_context));
            }
        }

        public IPlaylistItemRepository PlaylistItemRepository
        {
            get
            {
                return _getPlaylistItemRepository
                    ?? (_getPlaylistItemRepository = new PlaylistItemRepository(_context));
            }
        }

        public IRatingRepository RatingRepository
        {
            get
            {
                return _getRatingRepository
                    ?? (_getRatingRepository = new RatingRepository(_context));
            }
        }

        public IRoleRepository RoleRepository
        {
            get
            {
                return _getRoleRepository ?? (_getRoleRepository = new RoleRepository(_context));
            }
        }

        public ISeriesRepository SeriesRepository
        {
            get
            {
                return _getSeriesRepository
                    ?? (_getSeriesRepository = new SeriesRepository(_context));
            }
        }

        public ISeriesActorRepository SeriesActorRepository
        {
            get
            {
                return _getSeriesActorRepository
                    ?? (_getSeriesActorRepository = new SeriesActorRepository(_context));
            }
        }

        public ISeriesCategoryRepository SeriesCategoryRepository
        {
            get
            {
                return _getSeriesCategoryRepository
                    ?? (_getSeriesCategoryRepository = new SeriesCategoryRepository(_context));
            }
        }

        public ISeriesDirectorRepository SeriesDirectorRepository
        {
            get
            {
                return _getSeriesDirectorRepository
                    ?? (_getSeriesDirectorRepository = new SeriesDirectorRepository(_context));
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                return _getUserRepository ?? (_getUserRepository = new UserRepository(_context));
            }
        }

        public IUserRoleRepository UserRoleRepository
        {
            get
            {
                return _getUserRoleRepository
                    ?? (_getUserRoleRepository = new UserRoleRepository(_context));
            }
        }

        public IUserTokenRepository UserTokenRepository
        {
            get
            {
                return _getUserTokenRepository
                    ?? (_getUserTokenRepository = new UserTokenRepository(_context));
            }
        }

        public IVideoQualityRepository VideoQualityRepository
        {
            get
            {
                return _getVideoQualityRepository
                    ?? (_getVideoQualityRepository = new VideoQualityRepository(_context));
            }
        }

        public IVideoServerRepository VideoServerRepository
        {
            get
            {
                return _getVideoServerRepository
                    ?? (_getVideoServerRepository = new VideoServerRepository(_context));
            }
        }

        public IWatchHistoryRepository WatchHistoryRepository
        {
            get
            {
                return _getWatchHistoryRepository
                    ?? (_getWatchHistoryRepository = new WatchHistoryRepository(_context));
            }
        }

        public IUnifiedMovieRepository UnifiedMovieRepository
        {
            get
            {
                return _getUnifiedMovieRepository
                    ?? (_getUnifiedMovieRepository = new UnifiedMovieRepository(_context));
            }
        }
    }
}
