using GameStore.BusinessLogic.Repositories;
using Business = GameStore.BusinessLogic.Models;
using Entity = GameStore.DataAccess.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using GameStore.DataAccess.Models;
using System.Net.Http.Headers;
using GameStore.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DataAccess.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly Entity.GameStoreContext _context;

        public GameRepository(Entity.GameStoreContext context)
        {
            _context = context;
        }



        public Guid AddNewGame(Business.FullGame game)
        {
            _context.Game.Add(BusinessToEntityGame(game));
            _context.SaveChanges();

            BusinessToEntityGameGenre(game);

            BusinessToEntityGamePlatform(game);

            return game.game.Id;
        }

        public Business.Game GetGameByKey(string key)
        {
            return EntityToBusinessGame(_context.Game.Where(g => g.Key == key).SingleOrDefault() ?? throw new KeyNotFoundException("Game Key not found"));
        }

        public Business.Game GetGameByGameId(Guid gameId)
        {
            return EntityToBusinessGame(_context.Game.Where(g => g.Id == gameId).SingleOrDefault() ?? throw new KeyNotFoundException("Game Id not found"));
        }

        public List<Business.Game> GetGamesByPlatformId(Guid platformId)
        {
            List<Business.Game> gameList = new List<Business.Game>();
            List<Entity.GamePlatform> gamePlatform = _context.GamePlatform.Where(g => g.Platform == platformId).ToList();
            foreach (var item in gamePlatform)
            {
                gameList.Add(GetGameByGameId(item.GameId));
            }

            return gameList;
        }

        public List<Business.Game> GetGamesByGenreId(Guid genreId)
        {
            List<Business.Game> gameList = new List<Business.Game>();
            List<Entity.GameGenre> gameGenre = _context.GameGenre.Where(g => g.GenreId == genreId).ToList();
            foreach (var item in gameGenre)
            {
                gameList.Add(GetGameByGameId(item.GameId));
            }

            return gameList;
        }

        public Business.FullGame GetFullGame(Guid gameId)
        {

            Entity.Game game = _context.Game.Where(g => g.Id == gameId).SingleOrDefault() ?? throw new KeyNotFoundException("Game Id not found");
            List<Entity.GameGenre> gameGenres = _context.GameGenre.Where(g => g.GameId == gameId).ToList();
            List<Entity.GamePlatform> gamePlatforms = _context.GamePlatform.Where(g => g.GameId == gameId).ToList();

            return EntityToBusinessFullGame(game, gameGenres, gamePlatforms);
        }

        public Guid UpdateGame(Business.FullGame game)
        {
            _ = _context.Game.Where(g => g.Id == game.game.Id).SingleOrDefault() ?? throw new Exception("No such Game");

            FindAndRemoveGameGenres(game.game.Id);
            FindAndRemoveGamePlatforms(game.game.Id);

            _context.Game.Update(BusinessToEntityGame(game));
            _context.SaveChanges();

            BusinessToEntityGameGenre(game);

            BusinessToEntityGamePlatform(game);

            return game.game.Id;
        }

        public void DeleteGame(string gameKey)
        {
            Entity.Game game = _context.Game.Where(g => g.Key == gameKey).SingleOrDefault() ?? throw new KeyNotFoundException("Can't find Game Key");

            //FindAndRemoveGameGenres(game.Id);

            //FindAndRemoveGamePlatforms(game.Id);

            _context.Game.Remove(game);
            _context.SaveChanges();
        }

        public List<Business.Game> GetAllGames()
        {
            List<Entity.Game> entityGamesList = _context.Game.ToList();
            List<Business.Game> businessGamesList = new List<Business.Game>();

            foreach (var item in entityGamesList)
            {
                businessGamesList.Add(EntityToBusinessGame(item));
            }

            return businessGamesList;
        }



        public Guid AddNewGenre(Business.Genre businessGenre)
        {
            var genreCheck = _context.Genre.Where(g => g.Id == businessGenre.Id).SingleOrDefault();

            if (genreCheck.Id == businessGenre.Id)
            {
                businessGenre.Id = Guid.NewGuid();
            }

            if (_context.Genre.Where(g => g.Name == businessGenre.Name).SingleOrDefault().Name == businessGenre.Name)
            {
                throw new ArgumentException("Genre Name already exists");
            }

            Entity.Genre genre = new Entity.Genre();
            genre.Id = businessGenre.Id;
            genre.Name = businessGenre.Name;
            genre.ParentGenreId = businessGenre.ParentGenreId;

            _context.Genre.Add(genre);
            _context.SaveChanges();
            _context.Entry(genre).State = EntityState.Detached;

            return genre.Id;
        }

        public Business.Genre GetGenreById(Guid genreId)
        {
            var genre = _context.Genre.Where(g => g.Id == genreId).SingleOrDefault();
            ArgumentNullException.ThrowIfNull(genre);

            return EntityToBusinessGenre(genre);
        }

        public List<Business.Genre> GetAllGenres()
        {
            List<Business.Genre> businessGenres = new List<Business.Genre>();
            var genres = _context.Genre.ToList();

            foreach (var item in genres)
            {
                businessGenres.Add(EntityToBusinessGenre(item));
            }

            return businessGenres;
        }

        public List<Business.Genre> GetGenresByGameId(Guid gameId)
        {
            var gameGenreList = _context.GameGenre.Where(g => g.GameId == gameId).ToList();
            List<Entity.Genre> genres = new List<Entity.Genre>();
            List<Business.Genre> businessGenres = new List<Business.Genre>();

            foreach (var item in gameGenreList)
            {
                genres.Add(_context.Genre.Where(g => g.Id == item.GenreId).SingleOrDefault());
            }

            foreach (var item in genres)
            {
                businessGenres.Add(EntityToBusinessGenre(item));
            }

            return businessGenres;
        }

        public List<Business.Genre> GetGenresByParentId(Guid parentId)
        {
            var genres = _context.Genre.Where(g => g.ParentGenreId == parentId).ToList();
            List<Business.Genre> businessGenres = new List<Business.Genre>();

            foreach (var item in genres)
            {
                businessGenres.Add(EntityToBusinessGenre(item));
            }

            return businessGenres;
        }

        public Guid UpdateGenre(Business.Genre genre)
        {
            var entityGenre = BusinessToEntityGenre(genre);
            _context.Genre.Update(entityGenre);
            _context.SaveChanges();
            _context.Entry(entityGenre).State = EntityState.Detached;

            return genre.Id;
        }

        public Guid DeleteGenre(Guid genreId)
        {
            //var list = _context.GameGenre.Where(g => g.GenreId == genreId).ToList();
            //_context.GameGenre.RemoveRange(list);

            var genre = _context.Genre.Where(g => g.Id == genreId).FirstOrDefault();
            _context.Genre.Remove(genre);

            return genreId;
        }



        public Guid CreatePlatform(Business.Platform platform)
        {
            _context.Platform.Add(BusinessToEntityPlatform(platform));
            _context.SaveChanges();
            _context.Entry(platform).State = EntityState.Detached;

            return platform.Id;
        }

        public Business.Platform GetPlatformById(Guid id)
        {
            var platform = _context.Platform.Where(g => g.Id == id).SingleOrDefault();
            return EntityToBusinessPlatform(platform);
        }

        public List<Business.Platform> GetAllPlatforms()
        {
            var entityPlatforms = _context.Platform.ToList();
            List<Business.Platform> businessPlatforms = new List<Business.Platform>();

            foreach (var item in entityPlatforms)
            {
                businessPlatforms.Add(EntityToBusinessPlatform(item));
            }

            return businessPlatforms;
        }

        public List<Business.Platform> GetPlatformsByGameId(Guid gameId)
        {
            var list = _context.GamePlatform.Where(g => g.GameId == gameId).ToList();
            List<Entity.Platform> platforms = new List<Entity.Platform>();

            foreach (var item in list)
            {
                platforms.Add(_context.Platform.Where(g => g.Id == item.Platform).SingleOrDefault());
            }

            List<Business.Platform> businessPlatforms = new List<Business.Platform>();

            foreach(var item in platforms)
            {
                businessPlatforms.Add(EntityToBusinessPlatform(item));
            }

            return businessPlatforms;
        }

        public Guid UpdatePlatform(Business.Platform platform)
        {
            _context.Platform.Update(BusinessToEntityPlatform(platform));
            _context.SaveChanges();
            _context.Entry(platform).State = EntityState.Detached;

            return platform.Id;
        }

        public void DeletePlatform(Guid id)
        {
            //_context.GamePlatform.RemoveRange(_context.GamePlatform.Where(g => g.Platform == id).ToList());
            //_context.SaveChanges();

            _context.Platform.Remove(_context.Platform.Where(g => g.Id == id).SingleOrDefault()); 
            _context.SaveChanges();
        }




        private Business.FullGame EntityToBusinessFullGame(Entity.Game game, List<Entity.GameGenre> gameGenres, List<Entity.GamePlatform> gamePlatforms)
        {
            Business.FullGame fullGame = new Business.FullGame();
            fullGame.game = EntityToBusinessGame(game);

            foreach (var item in gameGenres)
            {
                fullGame.genres.Add(item.GenreId);
            }

            foreach (var item in gamePlatforms)
            {
                fullGame.platforms.Add(item.Platform);
            }

            return fullGame;
        }

        private Business.Game EntityToBusinessGame(Entity.Game entityGame)
        {
            Business.Game businessGame = new Business.Game();
            businessGame.Id = entityGame.Id;
            businessGame.Name = entityGame.Name;
            businessGame.Key = entityGame.Key;
            businessGame.Description = entityGame.Description;

            return businessGame;
        }

        private Business.Genre EntityToBusinessGenre(Entity.Genre entityGenre)
        {
            Business.Genre businessGenre = new Business.Genre();
            businessGenre.Id = entityGenre.Id;
            businessGenre.Name = entityGenre.Name;
            businessGenre.ParentGenreId = entityGenre.ParentGenreId;

            return businessGenre;
        }

        private Business.Platform EntityToBusinessPlatform(Entity.Platform entityPlatform)
        {
            Business.Platform businessPlatform = new Business.Platform();
            businessPlatform.Id = entityPlatform.Id;
            businessPlatform.Type = entityPlatform.Type;

            return businessPlatform;
        }



        private void FindAndRemoveGameGenres(Guid gameId)
        {
            List<Entity.GameGenre> gameGenres = _context.GameGenre.Where(g => g.GameId == gameId).ToList();
            _context.GameGenre.RemoveRange(gameGenres);
            _context.SaveChanges();
        }

        private void FindAndRemoveGamePlatforms(Guid gameId)
        {
            List<Entity.GamePlatform> gamePlatforms = _context.GamePlatform.Where(g => g.GameId == gameId).ToList();
            _context.GamePlatform.RemoveRange(gamePlatforms);
            _context.SaveChanges();
        }



        private Entity.Game BusinessToEntityGame(Business.FullGame businessGame)
        {
            Entity.Game entityGame = new Entity.Game();
            entityGame.Id = businessGame.game.Id;
            entityGame.Name = businessGame.game.Name;
            entityGame.Key = businessGame.game.Key;
            entityGame.Description = businessGame.game.Description;

            return entityGame;
        }

        private void BusinessToEntityGameGenre(Business.FullGame businessGame)
        {
            foreach (var item in businessGame.genres)
            {
                Entity.GameGenre gameGenre = new Entity.GameGenre();
                gameGenre.GameId = businessGame.game.Id;
                gameGenre.GenreId = item;
                AddGameGenre(gameGenre);
            }
        }

        private void BusinessToEntityGamePlatform(Business.FullGame businessGame)
        {
            foreach (var item in businessGame.platforms)
            {
                Entity.GamePlatform gamePlatform = new Entity.GamePlatform();
                gamePlatform.GameId = businessGame.game.Id;
                gamePlatform.Platform = item;
                AddGamePlatform(gamePlatform);
            }
        }

        private void AddGameGenre(Entity.GameGenre gameGenre)
        {
            _context.GameGenre.Add(gameGenre);
            _context.SaveChanges();
            _context.Entry(gameGenre).State = EntityState.Detached;
        }

        private void AddGamePlatform(Entity.GamePlatform gamePlatform)
        {
            _context.GamePlatform.Add(gamePlatform);
            _context.SaveChanges();
            _context.Entry(gamePlatform).State = EntityState.Detached;
        }

        private Entity.Genre BusinessToEntityGenre(Business.Genre genre)
        {
            Entity.Genre entityGenre = new Entity.Genre();
            entityGenre.Id = genre.Id;
            entityGenre.Name = genre.Name;
            entityGenre.ParentGenreId = genre.ParentGenreId;

            return entityGenre;
        }

        private Entity.Platform BusinessToEntityPlatform(Business.Platform businessPlatform)
        {
            Entity.Platform platform = new Entity.Platform();
            platform.Id = businessPlatform.Id;
            platform.Type = businessPlatform.Type;

            return platform;
        }
    }
}
