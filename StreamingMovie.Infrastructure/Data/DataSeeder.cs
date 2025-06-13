using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Infrastructure.Data
{
    public class DatabaseSeeder
    {
        private readonly MovieDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public DatabaseSeeder(
            MovieDbContext context,
            UserManager<User> userManager,
            RoleManager<Role> roleManager
        )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> SeedAllAsync()
        {
            // Seed in order of dependencies
            await SeedRolesAsync();
            await SeedCountriesAsync();
            await SeedCategoriesAsync();
            await SeedVideoQualitiesAsync();
            await SeedVideoServersAsync();
            await SeedActorsAsync();
            await SeedDirectorsAsync();
            //await SeedUsersAsync();
            await SeedMoviesAsync();
            await SeedSeriesAsync();
            await SeedEpisodesAsync();
            await SeedMovieVideosAsync();
            await SeedEpisodeVideosAsync();
            await SeedMovieCategoriesAsync();
            await SeedMovieActorsAsync();
            await SeedMovieDirectorsAsync();
            await SeedSeriesCategoriesAsync();
            await SeedSeriesActorsAsync();
            await SeedSeriesDirectorsAsync();
            await SeedFavoritesAsync();
            await SeedRatingsAsync();
            await SeedCommentsAsync();
            await SeedPlaylistsAsync();
            await SeedPlaylistItemsAsync();
            await SeedNotificationsAsync();
            await SeedWatchHistoriesAsync();

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                return true;
            }
        }

        private async Task SeedRolesAsync()
        {
            if (!await _roleManager.Roles.AnyAsync())
            {
                var roles = new List<Role>
                {
                    new Role { Name = "Admin", NormalizedName = "ADMIN" },
                    new Role { Name = "User", NormalizedName = "USER" },
                    new Role { Name = "Premium", NormalizedName = "PREMIUM" }
                };

                foreach (var role in roles)
                {
                    await _roleManager.CreateAsync(role);
                }
            }
        }

        private async Task SeedCountriesAsync()
        {
            if (!await _context.Countries.AnyAsync())
            {
                var countries = new List<Country>
                {
                    new Country { Name = "United States", Code = "US" },
                    new Country { Name = "United Kingdom", Code = "UK" },
                    new Country { Name = "Japan", Code = "JP" },
                    new Country { Name = "South Korea", Code = "KR" },
                    new Country { Name = "France", Code = "FR" },
                    new Country { Name = "Germany", Code = "DE" },
                    new Country { Name = "Canada", Code = "CA" },
                    new Country { Name = "Australia", Code = "AU" },
                    new Country { Name = "India", Code = "IN" },
                    new Country { Name = "China", Code = "CN" },
                    new Country { Name = "Vietnam", Code = "VN" },
                    new Country { Name = "Italy", Code = "IT" },
                    new Country { Name = "Spain", Code = "ES" },
                    new Country { Name = "Brazil", Code = "BR" },
                    new Country { Name = "Mexico", Code = "MX" },
                    new Country { Name = "Russia", Code = "RU" },
                    new Country { Name = "Thailand", Code = "TH" },
                    new Country { Name = "Turkey", Code = "TR" },
                    new Country { Name = "Netherlands", Code = "NL" },
                    new Country { Name = "Sweden", Code = "SE" }
                };

                await _context.Countries.AddRangeAsync(countries);
            }
        }

        private async Task SeedCategoriesAsync()
        {
            if (!await _context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category
                    {
                        Name = "Action",
                        Description = "High-energy movies and series with intense sequences",
                        Slug = "action"
                    },
                    new Category
                    {
                        Name = "Comedy",
                        Description = "Humorous content designed to entertain and amuse",
                        Slug = "comedy"
                    },
                    new Category
                    {
                        Name = "Drama",
                        Description = "Serious, plot-driven presentations of realistic characters",
                        Slug = "drama"
                    },
                    new Category
                    {
                        Name = "Horror",
                        Description = "Scary content designed to frighten and create suspense",
                        Slug = "horror"
                    },
                    new Category
                    {
                        Name = "Romance",
                        Description = "Love stories and romantic relationships",
                        Slug = "romance"
                    },
                    new Category
                    {
                        Name = "Thriller",
                        Description = "Suspenseful content that keeps you on the edge",
                        Slug = "thriller"
                    },
                    new Category
                    {
                        Name = "Sci-Fi",
                        Description = "Science fiction with futuristic concepts",
                        Slug = "sci-fi"
                    },
                    new Category
                    {
                        Name = "Fantasy",
                        Description = "Magical and supernatural elements",
                        Slug = "fantasy"
                    },
                    new Category
                    {
                        Name = "Animation",
                        Description = "Animated movies and series for all ages",
                        Slug = "animation"
                    },
                    new Category
                    {
                        Name = "Documentary",
                        Description = "Non-fiction content about real events",
                        Slug = "documentary"
                    },
                    new Category
                    {
                        Name = "Crime",
                        Description = "Stories involving criminal activities",
                        Slug = "crime"
                    },
                    new Category
                    {
                        Name = "Adventure",
                        Description = "Exciting journeys and explorations",
                        Slug = "adventure"
                    },
                    new Category
                    {
                        Name = "Mystery",
                        Description = "Puzzling stories that need to be solved",
                        Slug = "mystery"
                    },
                    new Category
                    {
                        Name = "War",
                        Description = "Military conflicts and wartime stories",
                        Slug = "war"
                    },
                    new Category
                    {
                        Name = "Western",
                        Description = "Stories set in the American Old West",
                        Slug = "western"
                    },
                    new Category
                    {
                        Name = "Musical",
                        Description = "Content featuring music and songs",
                        Slug = "musical"
                    },
                    new Category
                    {
                        Name = "Biography",
                        Description = "Life stories of real people",
                        Slug = "biography"
                    },
                    new Category
                    {
                        Name = "History",
                        Description = "Historical events and periods",
                        Slug = "history"
                    },
                    new Category
                    {
                        Name = "Family",
                        Description = "Content suitable for the whole family",
                        Slug = "family"
                    },
                    new Category
                    {
                        Name = "Sport",
                        Description = "Sports-related content",
                        Slug = "sport"
                    }
                };

                await _context.Categories.AddRangeAsync(categories);
            }
        }

        private async Task SeedVideoQualitiesAsync()
        {
            if (!await _context.VideoQualities.AnyAsync())
            {
                var qualities = new List<VideoQuality>
                {
                    new VideoQuality
                    {
                        Name = "SD",
                        Resolution = "480p",
                        Bitrate = 800
                    },
                    new VideoQuality
                    {
                        Name = "HD",
                        Resolution = "720p",
                        Bitrate = 1500
                    },
                    new VideoQuality
                    {
                        Name = "Full HD",
                        Resolution = "1080p",
                        Bitrate = 3000
                    },
                    new VideoQuality
                    {
                        Name = "4K",
                        Resolution = "2160p",
                        Bitrate = 8000
                    }
                };

                await _context.VideoQualities.AddRangeAsync(qualities);
            }
        }

        private async Task SeedVideoServersAsync()
        {
            if (!await _context.VideoServers.AnyAsync())
            {
                var servers = new List<VideoServer>
                {
                    new VideoServer
                    {
                        Name = "Server 1",
                        BaseUrl = "https://stream1.example.com",
                        IsActive = true,
                        Priority = 1
                    },
                    new VideoServer
                    {
                        Name = "Server 2",
                        BaseUrl = "https://stream2.example.com",
                        IsActive = true,
                        Priority = 2
                    },
                    new VideoServer
                    {
                        Name = "Server 3",
                        BaseUrl = "https://stream3.example.com",
                        IsActive = true,
                        Priority = 3
                    },
                    new VideoServer
                    {
                        Name = "Backup Server",
                        BaseUrl = "https://backup.example.com",
                        IsActive = false,
                        Priority = 4
                    }
                };

                await _context.VideoServers.AddRangeAsync(servers);
            }
        }

        private async Task SeedActorsAsync()
        {
            if (!await _context.Actors.AnyAsync())
            {
                var actors = new List<Actor>
                {
                    // Hollywood A-listers
                    new Actor
                    {
                        Name = "Robert Downey Jr.",
                        Biography = "American actor and producer known for playing Iron Man",
                        DateOfBirth = new DateTime(1965, 4, 4),
                        Nationality = "American",
                        AvatarUrl = "https://cdn.movieflix.com/actors/rdj.jpg"
                    },
                    new Actor
                    {
                        Name = "Scarlett Johansson",
                        Biography = "American actress and singer, known for Black Widow",
                        DateOfBirth = new DateTime(1984, 11, 22),
                        Nationality = "American",
                        AvatarUrl = "https://cdn.movieflix.com/actors/sj.jpg"
                    },
                    new Actor
                    {
                        Name = "Leonardo DiCaprio",
                        Biography = "American actor and film producer, Oscar winner",
                        DateOfBirth = new DateTime(1974, 11, 11),
                        Nationality = "American",
                        AvatarUrl = "https://cdn.movieflix.com/actors/ld.jpg"
                    },
                    new Actor
                    {
                        Name = "Jennifer Lawrence",
                        Biography = "American actress, Oscar winner for Silver Linings Playbook",
                        DateOfBirth = new DateTime(1990, 8, 15),
                        Nationality = "American",
                        AvatarUrl = "https://cdn.movieflix.com/actors/jl.jpg"
                    },
                    new Actor
                    {
                        Name = "Tom Hanks",
                        Biography = "American actor and filmmaker, two-time Oscar winner",
                        DateOfBirth = new DateTime(1956, 7, 9),
                        Nationality = "American",
                        AvatarUrl = "https://cdn.movieflix.com/actors/th.jpg"
                    },
                    new Actor
                    {
                        Name = "Meryl Streep",
                        Biography = "American actress, three-time Oscar winner",
                        DateOfBirth = new DateTime(1949, 6, 22),
                        Nationality = "American",
                        AvatarUrl = "https://cdn.movieflix.com/actors/ms.jpg"
                    },
                    new Actor
                    {
                        Name = "Denzel Washington",
                        Biography = "American actor and director, two-time Oscar winner",
                        DateOfBirth = new DateTime(1954, 12, 28),
                        Nationality = "American",
                        AvatarUrl = "https://cdn.movieflix.com/actors/dw.jpg"
                    },
                    new Actor
                    {
                        Name = "Morgan Freeman",
                        Biography = "American actor and narrator, Oscar winner",
                        DateOfBirth = new DateTime(1937, 6, 1),
                        Nationality = "American",
                        AvatarUrl = "https://cdn.movieflix.com/actors/mf.jpg"
                    },
                    new Actor
                    {
                        Name = "Christian Bale",
                        Biography = "English actor known for method acting",
                        DateOfBirth = new DateTime(1974, 1, 30),
                        Nationality = "British",
                        AvatarUrl = "https://cdn.movieflix.com/actors/cb.jpg"
                    },
                    new Actor
                    {
                        Name = "Ryan Gosling",
                        Biography = "Canadian actor known for La La Land and Drive",
                        DateOfBirth = new DateTime(1980, 11, 12),
                        Nationality = "Canadian",
                        AvatarUrl = "https://cdn.movieflix.com/actors/rg.jpg"
                    },
                    // International stars
                    new Actor
                    {
                        Name = "Song Kang-ho",
                        Biography = "South Korean actor known for Parasite",
                        DateOfBirth = new DateTime(1967, 1, 17),
                        Nationality = "South Korean",
                        AvatarUrl = "https://cdn.movieflix.com/actors/skh.jpg"
                    },
                    new Actor
                    {
                        Name = "Toshiro Mifune",
                        Biography = "Japanese actor known for samurai films",
                        DateOfBirth = new DateTime(1920, 4, 1),
                        Nationality = "Japanese",
                        AvatarUrl = "https://cdn.movieflix.com/actors/tm.jpg"
                    },
                    new Actor
                    {
                        Name = "Marion Cotillard",
                        Biography = "French actress, Oscar winner for La Vie en Rose",
                        DateOfBirth = new DateTime(1975, 9, 30),
                        Nationality = "French",
                        AvatarUrl = "https://cdn.movieflix.com/actors/mc.jpg"
                    },
                    new Actor
                    {
                        Name = "Javier Bardem",
                        Biography = "Spanish actor, Oscar winner for No Country for Old Men",
                        DateOfBirth = new DateTime(1969, 3, 1),
                        Nationality = "Spanish",
                        AvatarUrl = "https://cdn.movieflix.com/actors/jb.jpg"
                    },
                    new Actor
                    {
                        Name = "Cate Blanchett",
                        Biography = "Australian actress, two-time Oscar winner",
                        DateOfBirth = new DateTime(1969, 5, 14),
                        Nationality = "Australian",
                        AvatarUrl = "https://cdn.movieflix.com/actors/cb2.jpg"
                    },
                    // Rising stars and TV actors
                    new Actor
                    {
                        Name = "Zendaya",
                        Biography = "American actress and singer, Emmy winner for Euphoria",
                        DateOfBirth = new DateTime(1996, 9, 1),
                        Nationality = "American",
                        AvatarUrl = "https://cdn.movieflix.com/actors/z.jpg"
                    },
                    new Actor
                    {
                        Name = "Bryan Cranston",
                        Biography = "American actor known for Breaking Bad",
                        DateOfBirth = new DateTime(1956, 3, 7),
                        Nationality = "American",
                        AvatarUrl = "https://cdn.movieflix.com/actors/bc.jpg"
                    },
                    new Actor
                    {
                        Name = "Aaron Paul",
                        Biography = "American actor known for Breaking Bad",
                        DateOfBirth = new DateTime(1979, 8, 27),
                        Nationality = "American",
                        AvatarUrl = "https://cdn.movieflix.com/actors/ap.jpg"
                    },
                    new Actor
                    {
                        Name = "Millie Bobby Brown",
                        Biography = "British actress known for Stranger Things",
                        DateOfBirth = new DateTime(2004, 2, 19),
                        Nationality = "British",
                        AvatarUrl = "https://cdn.movieflix.com/actors/mbb.jpg"
                    },
                    new Actor
                    {
                        Name = "Finn Wolfhard",
                        Biography = "Canadian actor known for Stranger Things",
                        DateOfBirth = new DateTime(2002, 12, 23),
                        Nationality = "Canadian",
                        AvatarUrl = "https://cdn.movieflix.com/actors/fw.jpg"
                    }
                };

                await _context.Actors.AddRangeAsync(actors);
            }
        }

        private async Task SeedDirectorsAsync()
        {
            if (!await _context.Directors.AnyAsync())
            {
                var directors = new List<Director>
                {
                    new Director
                    {
                        Name = "Christopher Nolan",
                        Biography = "British-American film director known for complex narratives",
                        DateOfBirth = new DateTime(1970, 7, 30),
                        Nationality = "British",
                        AvatarUrl = "https://cdn.movieflix.com/directors/cn.jpg"
                    },
                    new Director
                    {
                        Name = "Steven Spielberg",
                        Biography = "American filmmaker, three-time Oscar winner",
                        DateOfBirth = new DateTime(1946, 12, 18),
                        Nationality = "American",
                        AvatarUrl = "https://cdn.movieflix.com/directors/ss.jpg"
                    },
                    new Director
                    {
                        Name = "Quentin Tarantino",
                        Biography = "American film director known for nonlinear storylines",
                        DateOfBirth = new DateTime(1963, 3, 27),
                        Nationality = "American",
                        AvatarUrl = "https://cdn.movieflix.com/directors/qt.jpg"
                    },
                    new Director
                    {
                        Name = "Martin Scorsese",
                        Biography = "American film director, Oscar winner for The Departed",
                        DateOfBirth = new DateTime(1942, 11, 17),
                        Nationality = "American",
                        AvatarUrl = "https://cdn.movieflix.com/directors/ms.jpg"
                    },
                    new Director
                    {
                        Name = "Denis Villeneuve",
                        Biography = "Canadian filmmaker known for sci-fi films",
                        DateOfBirth = new DateTime(1967, 10, 3),
                        Nationality = "Canadian",
                        AvatarUrl = "https://cdn.movieflix.com/directors/dv.jpg"
                    },
                    new Director
                    {
                        Name = "Bong Joon-ho",
                        Biography = "South Korean filmmaker, Oscar winner for Parasite",
                        DateOfBirth = new DateTime(1969, 9, 14),
                        Nationality = "South Korean",
                        AvatarUrl = "https://cdn.movieflix.com/directors/bjh.jpg"
                    },
                    new Director
                    {
                        Name = "Greta Gerwig",
                        Biography = "American director known for Lady Bird and Little Women",
                        DateOfBirth = new DateTime(1983, 8, 4),
                        Nationality = "American",
                        AvatarUrl = "https://cdn.movieflix.com/directors/gg.jpg"
                    },
                    new Director
                    {
                        Name = "Jordan Peele",
                        Biography = "American director known for horror films",
                        DateOfBirth = new DateTime(1979, 2, 21),
                        Nationality = "American",
                        AvatarUrl = "https://cdn.movieflix.com/directors/jp.jpg"
                    },
                    new Director
                    {
                        Name = "Rian Johnson",
                        Biography = "American director known for Knives Out",
                        DateOfBirth = new DateTime(1973, 12, 17),
                        Nationality = "American",
                        AvatarUrl = "https://cdn.movieflix.com/directors/rj.jpg"
                    },
                    new Director
                    {
                        Name = "Chloé Zhao",
                        Biography = "Chinese filmmaker, Oscar winner for Nomadland",
                        DateOfBirth = new DateTime(1982, 3, 31),
                        Nationality = "Chinese",
                        AvatarUrl = "https://cdn.movieflix.com/directors/cz.jpg"
                    }
                };

                await _context.Directors.AddRangeAsync(directors);
            }
        }

        private async Task SeedUsersAsync()
        {
            if (!await _userManager.Users.AnyAsync())
            {
                var users = new List<(User user, string password, string role)>
                {
                    (
                        new User
                        {
                            UserName = "admin@streaming.com",
                            Email = "admin@streaming.com",
                            FullName = "System Administrator",
                            EmailConfirmed = true,
                            SubscriptionType = "Premium",
                            SubscriptionStartDate = DateTime.Now,
                            SubscriptionEndDate = DateTime.Now.AddYears(1),
                            IsActive = true
                        },
                        "Admin@123",
                        "Admin"
                    ),
                    (
                        new User
                        {
                            UserName = "john.doe@example.com",
                            Email = "john.doe@example.com",
                            FullName = "John Doe",
                            EmailConfirmed = true,
                            SubscriptionType = "Premium",
                            SubscriptionStartDate = DateTime.Now,
                            SubscriptionEndDate = DateTime.Now.AddMonths(6),
                            IsActive = true
                        },
                        "User@123",
                        "Premium"
                    ),
                    (
                        new User
                        {
                            UserName = "jane.smith@example.com",
                            Email = "jane.smith@example.com",
                            FullName = "Jane Smith",
                            EmailConfirmed = true,
                            SubscriptionType = "Basic",
                            SubscriptionStartDate = DateTime.Now,
                            SubscriptionEndDate = DateTime.Now.AddMonths(1),
                            IsActive = true
                        },
                        "User@123",
                        "User"
                    )
                };

                foreach (var (user, password, role) in users)
                {
                    var result = await _userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }
                }
            }
        }

        private async Task SeedMoviesAsync()
        {
            if (!await _context.Movies.AnyAsync())
            {
                var usCountry = await _context.Countries.FirstAsync(c => c.Code == "US");
                var ukCountry = await _context.Countries.FirstAsync(c => c.Code == "UK");
                var krCountry = await _context.Countries.FirstAsync(c => c.Code == "KR");

                var movies = new List<Movie>
                {
                    // Blockbusters
                    new Movie
                    {
                        Title = "The Dark Knight",
                        OriginalTitle = "The Dark Knight",
                        Slug = "the-dark-knight-2008",
                        Description =
                            "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests of his ability to fight injustice.",
                        PosterUrl = "https://cdn.movieflix.com/posters/dark-knight.jpg",
                        BannerUrl = "https://cdn.movieflix.com/banners/dark-knight.jpg",
                        TrailerUrl = "https://cdn.movieflix.com/trailers/dark-knight.mp4",
                        Duration = 152,
                        ReleaseDate = new DateTime(2008, 7, 18),
                        ImdbRating = 9.0f,
                        OurRating = 8.8f,
                        ViewCount = 150000,
                        Status = "Released",
                        IsPremium = false,
                        CountryId = usCountry.Id
                    },
                    new Movie
                    {
                        Title = "Inception",
                        OriginalTitle = "Inception",
                        Slug = "inception-2010",
                        Description =
                            "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.",
                        PosterUrl = "https://cdn.movieflix.com/posters/inception.jpg",
                        BannerUrl = "https://cdn.movieflix.com/banners/inception.jpg",
                        TrailerUrl = "https://cdn.movieflix.com/trailers/inception.mp4",
                        Duration = 148,
                        ReleaseDate = new DateTime(2010, 7, 16),
                        ImdbRating = 8.8f,
                        OurRating = 8.5f,
                        ViewCount = 120000,
                        Status = "Released",
                        IsPremium = true,
                        CountryId = usCountry.Id
                    },
                    new Movie
                    {
                        Title = "Interstellar",
                        OriginalTitle = "Interstellar",
                        Slug = "interstellar-2014",
                        Description =
                            "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.",
                        PosterUrl = "https://cdn.movieflix.com/posters/interstellar.jpg",
                        BannerUrl = "https://cdn.movieflix.com/banners/interstellar.jpg",
                        TrailerUrl = "https://cdn.movieflix.com/trailers/interstellar.mp4",
                        Duration = 169,
                        ReleaseDate = new DateTime(2014, 11, 7),
                        ImdbRating = 8.6f,
                        OurRating = 8.4f,
                        ViewCount = 98000,
                        Status = "Released",
                        IsPremium = true,
                        CountryId = usCountry.Id
                    },
                    new Movie
                    {
                        Title = "Parasite",
                        OriginalTitle = "기생충",
                        Slug = "parasite-2019",
                        Description =
                            "A poor family schemes to become employed by a wealthy family and infiltrate their household by posing as unrelated, highly qualified individuals.",
                        PosterUrl = "https://cdn.movieflix.com/posters/parasite.jpg",
                        BannerUrl = "https://cdn.movieflix.com/banners/parasite.jpg",
                        TrailerUrl = "https://cdn.movieflix.com/trailers/parasite.mp4",
                        Duration = 132,
                        ReleaseDate = new DateTime(2019, 5, 30),
                        ImdbRating = 8.5f,
                        OurRating = 8.7f,
                        ViewCount = 87000,
                        Status = "Released",
                        IsPremium = true,
                        CountryId = krCountry.Id
                    },
                    new Movie
                    {
                        Title = "Avengers: Endgame",
                        OriginalTitle = "Avengers: Endgame",
                        Slug = "avengers-endgame-2019",
                        Description =
                            "After the devastating events of Avengers: Infinity War, the universe is in ruins. With the help of remaining allies, the Avengers assemble once more.",
                        PosterUrl = "https://cdn.movieflix.com/posters/endgame.jpg",
                        BannerUrl = "https://cdn.movieflix.com/banners/endgame.jpg",
                        TrailerUrl = "https://cdn.movieflix.com/trailers/endgame.mp4",
                        Duration = 181,
                        ReleaseDate = new DateTime(2019, 4, 26),
                        ImdbRating = 8.4f,
                        OurRating = 8.2f,
                        ViewCount = 200000,
                        Status = "Released",
                        IsPremium = false,
                        CountryId = usCountry.Id
                    },
                    new Movie
                    {
                        Title = "Joker",
                        OriginalTitle = "Joker",
                        Slug = "joker-2019",
                        Description =
                            "A mentally troubled comedian is driven insane and turns to a life of crime and chaos in Gotham City.",
                        PosterUrl = "https://cdn.movieflix.com/posters/joker.jpg",
                        BannerUrl = "https://cdn.movieflix.com/banners/joker.jpg",
                        TrailerUrl = "https://cdn.movieflix.com/trailers/joker.mp4",
                        Duration = 122,
                        ReleaseDate = new DateTime(2019, 10, 4),
                        ImdbRating = 8.4f,
                        OurRating = 8.1f,
                        ViewCount = 145000,
                        Status = "Released",
                        IsPremium = true,
                        CountryId = usCountry.Id
                    },
                    // Classic movies
                    new Movie
                    {
                        Title = "The Shawshank Redemption",
                        OriginalTitle = "The Shawshank Redemption",
                        Slug = "shawshank-redemption-1994",
                        Description =
                            "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                        PosterUrl = "https://cdn.movieflix.com/posters/shawshank.jpg",
                        BannerUrl = "https://cdn.movieflix.com/banners/shawshank.jpg",
                        TrailerUrl = "https://cdn.movieflix.com/trailers/shawshank.mp4",
                        Duration = 142,
                        ReleaseDate = new DateTime(1994, 9, 23),
                        ImdbRating = 9.3f,
                        OurRating = 9.1f,
                        ViewCount = 250000,
                        Status = "Released",
                        IsPremium = false,
                        CountryId = usCountry.Id
                    },
                    new Movie
                    {
                        Title = "Pulp Fiction",
                        OriginalTitle = "Pulp Fiction",
                        Slug = "pulp-fiction-1994",
                        Description =
                            "The lives of two mob hitmen, a boxer, a gangster and his wife intertwine in four tales of violence and redemption.",
                        PosterUrl = "https://cdn.movieflix.com/posters/pulp-fiction.jpg",
                        BannerUrl = "https://cdn.movieflix.com/banners/pulp-fiction.jpg",
                        TrailerUrl = "https://cdn.movieflix.com/trailers/pulp-fiction.mp4",
                        Duration = 154,
                        ReleaseDate = new DateTime(1994, 10, 14),
                        ImdbRating = 8.9f,
                        OurRating = 8.7f,
                        ViewCount = 180000,
                        Status = "Released",
                        IsPremium = true,
                        CountryId = usCountry.Id
                    }
                };

                await _context.Movies.AddRangeAsync(movies);
            }
        }

        private async Task SeedSeriesAsync()
        {
            if (!await _context.Series.AnyAsync())
            {
                var usCountry = await _context.Countries.FirstAsync(c => c.Code == "US");

                var series = new List<Series>
                {
                    new Series
                    {
                        Title = "Breaking Bad",
                        OriginalTitle = "Breaking Bad",
                        Slug = "breaking-bad",
                        Description = "A high school chemistry teacher turned meth manufacturer.",
                        PosterUrl = "https://example.com/posters/breaking-bad.jpg",
                        BannerUrl = "https://example.com/banners/breaking-bad.jpg",
                        TrailerUrl = "https://example.com/trailers/breaking-bad.mp4",
                        TotalSeasons = 5,
                        TotalEpisodes = 62,
                        ReleaseDate = new DateTime(2008, 1, 20),
                        EndDate = new DateTime(2013, 9, 29),
                        ImdbRating = 9.5f,
                        OurRating = 9.2f,
                        ViewCount = 25000,
                        Status = "Completed",
                        IsPremium = true,
                        CountryId = usCountry.Id
                    },
                    new Series
                    {
                        Title = "Stranger Things",
                        OriginalTitle = "Stranger Things",
                        Slug = "stranger-things",
                        Description = "Kids in a small town face supernatural forces.",
                        PosterUrl = "https://example.com/posters/stranger-things.jpg",
                        BannerUrl = "https://example.com/banners/stranger-things.jpg",
                        TrailerUrl = "https://example.com/trailers/stranger-things.mp4",
                        TotalSeasons = 4,
                        TotalEpisodes = 34,
                        ReleaseDate = new DateTime(2016, 7, 15),
                        ImdbRating = 8.7f,
                        OurRating = 8.4f,
                        ViewCount = 30000,
                        Status = "Ongoing",
                        IsPremium = false,
                        CountryId = usCountry.Id
                    }
                };

                await _context.Series.AddRangeAsync(series);
            }
        }

        private async Task SeedEpisodesAsync()
        {
            if (!await _context.Episodes.AnyAsync())
            {
                var breakingBad = await _context.Series.FirstAsync(s => s.Slug == "breaking-bad");
                var strangerThings = await _context.Series.FirstAsync(s =>
                    s.Slug == "stranger-things"
                );

                var episodes = new List<Episode>
                {
                    // Breaking Bad Season 1
                    new Episode
                    {
                        SeriesId = breakingBad.Id,
                        SeasonNumber = 1,
                        EpisodeNumber = 1,
                        Title = "Pilot",
                        Description = "Walter White begins his journey into the world of meth.",
                        Duration = 58,
                        AirDate = new DateTime(2008, 1, 20),
                        ViewCount = 5000,
                        IsPremium = true
                    },
                    new Episode
                    {
                        SeriesId = breakingBad.Id,
                        SeasonNumber = 1,
                        EpisodeNumber = 2,
                        Title = "Cat's in the Bag...",
                        Description = "Walter and Jesse attempt to tie up loose ends.",
                        Duration = 48,
                        AirDate = new DateTime(2008, 1, 27),
                        ViewCount = 4800,
                        IsPremium = true
                    },
                    // Stranger Things Season 1
                    new Episode
                    {
                        SeriesId = strangerThings.Id,
                        SeasonNumber = 1,
                        EpisodeNumber = 1,
                        Title = "Chapter One: The Vanishing of Will Byers",
                        Description = "A young boy vanishes and supernatural forces are revealed.",
                        Duration = 49,
                        AirDate = new DateTime(2016, 7, 15),
                        ViewCount = 8000,
                        IsPremium = false
                    }
                };

                await _context.Episodes.AddRangeAsync(episodes);
            }
        }

        private async Task SeedMovieVideosAsync()
        {
            if (!await _context.MovieVideos.AnyAsync())
            {
                var movies = await _context.Movies.ToListAsync();
                var servers = await _context.VideoServers.ToListAsync();
                var qualities = await _context.VideoQualities.ToListAsync();

                var movieVideos = new List<MovieVideo>();

                foreach (var movie in movies)
                {
                    foreach (var server in servers.Where(s => s.IsActive == true))
                    {
                        foreach (var quality in qualities)
                        {
                            movieVideos.Add(
                                new MovieVideo
                                {
                                    MovieId = movie.Id,
                                    VideoServerId = server.Id,
                                    VideoQualityId = quality.Id,
                                    VideoUrl =
                                        $"{server.BaseUrl}/movies/{movie.Slug}/{quality.Resolution}.mp4",
                                    SubtitleUrl =
                                        $"{server.BaseUrl}/movies/{movie.Slug}/subtitles_en.vtt",
                                    Language = "en",
                                    IsActive = true
                                }
                            );
                        }
                    }
                }

                await _context.MovieVideos.AddRangeAsync(movieVideos);
            }
        }

        private async Task SeedEpisodeVideosAsync()
        {
            if (!await _context.EpisodeVideos.AnyAsync())
            {
                var episodes = await _context.Episodes.ToListAsync();
                var servers = await _context
                    .VideoServers.Where(s => s.IsActive == true)
                    .ToListAsync();
                var qualities = await _context.VideoQualities.ToListAsync();

                var episodeVideos = new List<EpisodeVideo>();

                foreach (var episode in episodes)
                {
                    foreach (var server in servers)
                    {
                        foreach (var quality in qualities)
                        {
                            episodeVideos.Add(
                                new EpisodeVideo
                                {
                                    EpisodeId = episode.Id,
                                    VideoServerId = server.Id,
                                    VideoQualityId = quality.Id,
                                    VideoUrl =
                                        $"{server.BaseUrl}/episodes/{episode.Id}/s{episode.SeasonNumber}e{episode.EpisodeNumber}_{quality.Resolution}.mp4",
                                    SubtitleUrl =
                                        $"{server.BaseUrl}/episodes/{episode.Id}/subtitles_en.vtt",
                                    Language = "en",
                                    IsActive = true
                                }
                            );
                        }
                    }
                }

                await _context.EpisodeVideos.AddRangeAsync(episodeVideos);
            }
        }

        private async Task SeedMovieCategoriesAsync()
        {
            if (!await _context.MovieCategories.AnyAsync())
            {
                var darkKnight = await _context.Movies.FirstAsync(m =>
                    m.Slug == "the-dark-knight-2008"
                );
                var inception = await _context.Movies.FirstAsync(m => m.Slug == "inception-2010");

                var actionCategory = await _context.Categories.FirstAsync(c => c.Slug == "action");
                var crimeCategory = await _context.Categories.FirstAsync(c => c.Slug == "crime");
                var sciFiCategory = await _context.Categories.FirstAsync(c => c.Slug == "sci-fi");
                var thrillerCategory = await _context.Categories.FirstAsync(c =>
                    c.Slug == "thriller"
                );

                var movieCategories = new List<MovieCategory>
                {
                    new MovieCategory { MovieId = darkKnight.Id, CategoryId = actionCategory.Id },
                    new MovieCategory { MovieId = darkKnight.Id, CategoryId = crimeCategory.Id },
                    new MovieCategory { MovieId = inception.Id, CategoryId = actionCategory.Id },
                    new MovieCategory { MovieId = inception.Id, CategoryId = sciFiCategory.Id },
                    new MovieCategory { MovieId = inception.Id, CategoryId = thrillerCategory.Id }
                };

                await _context.MovieCategories.AddRangeAsync(movieCategories);
            }
        }

        private async Task SeedMovieActorsAsync()
        {
            if (!await _context.MovieActors.AnyAsync())
            {
                var inception = await _context.Movies.FirstAsync(m => m.Slug == "inception-2010");
                var leonardo = await _context.Actors.FirstAsync(a => a.Name == "Leonardo DiCaprio");

                var movieActors = new List<MovieActor>
                {
                    new MovieActor
                    {
                        MovieId = inception.Id,
                        ActorId = leonardo.Id,
                        CharacterName = "Dom Cobb",
                        IsMainCharacter = true
                    }
                };

                await _context.MovieActors.AddRangeAsync(movieActors);
            }
        }

        private async Task SeedMovieDirectorsAsync()
        {
            if (!await _context.MovieDirectors.AnyAsync())
            {
                var darkKnight = await _context.Movies.FirstAsync(m =>
                    m.Slug == "the-dark-knight-2008"
                );
                var inception = await _context.Movies.FirstAsync(m => m.Slug == "inception-2010");
                var nolan = await _context.Directors.FirstAsync(d => d.Name == "Christopher Nolan");

                var movieDirectors = new List<MovieDirector>
                {
                    new MovieDirector { MovieId = darkKnight.Id, DirectorId = nolan.Id },
                    new MovieDirector { MovieId = inception.Id, DirectorId = nolan.Id }
                };

                await _context.MovieDirectors.AddRangeAsync(movieDirectors);
            }
        }

        private async Task SeedSeriesCategoriesAsync()
        {
            if (!await _context.SeriesCategories.AnyAsync())
            {
                var breakingBad = await _context.Series.FirstAsync(s => s.Slug == "breaking-bad");
                var strangerThings = await _context.Series.FirstAsync(s =>
                    s.Slug == "stranger-things"
                );

                var dramaCategory = await _context.Categories.FirstAsync(c => c.Slug == "drama");
                var crimeCategory = await _context.Categories.FirstAsync(c => c.Slug == "crime");
                var sciFiCategory = await _context.Categories.FirstAsync(c => c.Slug == "sci-fi");
                var thrillerCategory = await _context.Categories.FirstAsync(c =>
                    c.Slug == "thriller"
                );

                var seriesCategories = new List<SeriesCategory>
                {
                    new SeriesCategory { SeriesId = breakingBad.Id, CategoryId = dramaCategory.Id },
                    new SeriesCategory { SeriesId = breakingBad.Id, CategoryId = crimeCategory.Id },
                    new SeriesCategory
                    {
                        SeriesId = strangerThings.Id,
                        CategoryId = sciFiCategory.Id
                    },
                    new SeriesCategory
                    {
                        SeriesId = strangerThings.Id,
                        CategoryId = thrillerCategory.Id
                    }
                };

                await _context.SeriesCategories.AddRangeAsync(seriesCategories);
            }
        }

        private async Task SeedSeriesActorsAsync()
        {
            if (!await _context.SeriesActors.AnyAsync())
            {
                // You can add series actors here when you have the data
                // Similar pattern to MovieActors
            }
        }

        private async Task SeedSeriesDirectorsAsync()
        {
            if (!await _context.SeriesDirectors.AnyAsync())
            {
                // You can add series directors here when you have the data
                // Similar pattern to MovieDirectors
            }
        }

        private async Task SeedFavoritesAsync()
        {
            if (!await _context.Favorites.AnyAsync())
            {
                var user = await _userManager.Users.FirstAsync(u =>
                    u.Email == "john.doe@example.com"
                );
                var darkKnight = await _context.Movies.FirstAsync(m =>
                    m.Slug == "the-dark-knight-2008"
                );
                var breakingBad = await _context.Series.FirstAsync(s => s.Slug == "breaking-bad");

                var favorites = new List<Favorite>
                {
                    new Favorite { UserId = user.Id, MovieId = darkKnight.Id },
                    new Favorite { UserId = user.Id, SeriesId = breakingBad.Id }
                };

                await _context.Favorites.AddRangeAsync(favorites);
            }
        }

        private async Task SeedRatingsAsync()
        {
            if (!await _context.Ratings.AnyAsync())
            {
                var user = await _userManager.Users.FirstAsync(u =>
                    u.Email == "john.doe@example.com"
                );
                var darkKnight = await _context.Movies.FirstAsync(m =>
                    m.Slug == "the-dark-knight-2008"
                );

                var ratings = new List<Rating>
                {
                    new Rating
                    {
                        UserId = user.Id,
                        MovieId = darkKnight.Id,
                        RatingValue = 9,
                        Review = "Amazing movie with great acting and direction!"
                    }
                };

                await _context.Ratings.AddRangeAsync(ratings);
            }
        }

        private async Task SeedCommentsAsync()
        {
            if (!await _context.Comments.AnyAsync())
            {
                var user = await _userManager.Users.FirstAsync(u =>
                    u.Email == "john.doe@example.com"
                );
                var darkKnight = await _context.Movies.FirstAsync(m =>
                    m.Slug == "the-dark-knight-2008"
                );

                var comments = new List<Comment>
                {
                    new Comment
                    {
                        UserId = user.Id,
                        MovieId = darkKnight.Id,
                        Content = "This is one of the best superhero movies ever made!",
                        LikeCount = 15,
                        DislikeCount = 2,
                        IsApproved = true
                    }
                };

                await _context.Comments.AddRangeAsync(comments);
            }
        }

        private async Task SeedPlaylistsAsync()
        {
            if (!await _context.Playlists.AnyAsync())
            {
                var user = await _userManager.Users.FirstAsync(u =>
                    u.Email == "john.doe@example.com"
                );

                var playlists = new List<Playlist>
                {
                    new Playlist
                    {
                        UserId = user.Id,
                        Name = "My Favorite Movies",
                        Description = "Collection of my all-time favorite movies",
                        IsPublic = true
                    },
                    new Playlist
                    {
                        UserId = user.Id,
                        Name = "Watch Later",
                        Description = "Movies and series to watch later",
                        IsPublic = false
                    }
                };

                await _context.Playlists.AddRangeAsync(playlists);
            }
        }

        private async Task SeedPlaylistItemsAsync()
        {
            if (!await _context.PlaylistItems.AnyAsync())
            {
                var user = await _userManager.Users.FirstAsync(u =>
                    u.Email == "john.doe@example.com"
                );
                var playlist = await _context.Playlists.FirstAsync(p =>
                    p.UserId == user.Id && p.Name == "My Favorite Movies"
                );
                var darkKnight = await _context.Movies.FirstAsync(m =>
                    m.Slug == "the-dark-knight-2008"
                );
                var inception = await _context.Movies.FirstAsync(m => m.Slug == "inception-2010");

                var playlistItems = new List<PlaylistItem>
                {
                    new PlaylistItem
                    {
                        PlaylistId = playlist.Id,
                        MovieId = darkKnight.Id,
                        Position = 1
                    },
                    new PlaylistItem
                    {
                        PlaylistId = playlist.Id,
                        MovieId = inception.Id,
                        Position = 2
                    }
                };

                await _context.PlaylistItems.AddRangeAsync(playlistItems);
            }
        }

        private async Task SeedNotificationsAsync()
        {
            if (!await _context.Notifications.AnyAsync())
            {
                var user = await _userManager.Users.FirstAsync(u =>
                    u.Email == "john.doe@example.com"
                );

                var notifications = new List<Notification>
                {
                    new Notification
                    {
                        UserId = user.Id,
                        Title = "Welcome to Streaming Movie!",
                        Message = "Thank you for joining us. Enjoy unlimited streaming!",
                        Type = "Welcome",
                        IsRead = false
                    },
                    new Notification
                    {
                        UserId = user.Id,
                        Title = "New Episode Available",
                        Message = "New episode of Breaking Bad is now available!",
                        Type = "NewContent",
                        IsRead = true
                    }
                };

                await _context.Notifications.AddRangeAsync(notifications);
            }
        }

        private async Task SeedWatchHistoriesAsync()
        {
            if (!await _context.WatchHistories.AnyAsync())
            {
                var user = await _userManager.Users.FirstAsync(u =>
                    u.Email == "john.doe@example.com"
                );
                var darkKnight = await _context.Movies.FirstAsync(m =>
                    m.Slug == "the-dark-knight-2008"
                );
                var episode = await _context.Episodes.FirstAsync();

                var watchHistories = new List<WatchHistory>
                {
                    new WatchHistory
                    {
                        UserId = user.Id,
                        MovieId = darkKnight.Id,
                        WatchPosition = 3600, // 1 hour in seconds
                        WatchDuration = 7200, // 2 hours in seconds
                        IsCompleted = false,
                        LastWatchedAt = DateTime.Now.AddDays(-1)
                    },
                    new WatchHistory
                    {
                        UserId = user.Id,
                        EpisodeId = episode.Id,
                        WatchPosition = 2900, // 48 minutes in seconds
                        WatchDuration = 2900,
                        IsCompleted = true,
                        LastWatchedAt = DateTime.Now.AddHours(-2)
                    }
                };

                await _context.WatchHistories.AddRangeAsync(watchHistories);
            }
        }
    }
}
