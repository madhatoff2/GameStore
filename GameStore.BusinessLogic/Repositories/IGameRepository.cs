
using GameStore.BusinessLogic.Models;

namespace GameStore.BusinessLogic.Repositories
{
    public interface IGameRepository
    {
        public Guid AddNewGame(FullGame game);

        public Game GetGameByKey(string key);

        public Game GetGameByGameId(Guid gameId);

        public List<Game> GetGamesByPlatformId(Guid platformId);

        public List<Game> GetGamesByGenreId(Guid genreId);

        public FullGame GetFullGame(Guid gameId);

        public Guid UpdateGame(FullGame game);

        public void DeleteGame(string key);

        public List<Game> GetAllGames();

        public Guid AddNewGenre(Genre businessGenre);

        public Genre GetGenreById(Guid genreId);

        public List<Genre> GetAllGenres();

        public List<Genre> GetGenresByGameId(Guid gameId);

        public List<Genre> GetGenresByParentId(Guid parentId);

        public Guid UpdateGenre(Genre genre);

        public Guid DeleteGenre(Guid genreId);

        public Guid CreatePlatform(Platform platform);

        public Platform GetPlatformById(Guid id);

        public List<Platform> GetAllPlatforms();

        public List<Platform> GetPlatformsByGameId(Guid gameId);

        public Guid UpdatePlatform(Platform platform);

        public void DeletePlatform(Guid id);

    }
}
