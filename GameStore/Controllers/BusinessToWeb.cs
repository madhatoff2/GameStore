using GameStore.Models;
using Entity = GameStore.DataAccess.Models;
using Business = GameStore.BusinessLogic.Models;

namespace GameStore.Controllers
{
    public class BusinessToWeb
    {
        public static Game BusinessToWebGame(Business.Game businessGame)
        {
            Game game = new Game();
            game.Id = businessGame.Id;
            game.Name = businessGame.Name;
            game.Key = businessGame.Key;
            game.Description = businessGame.Description;

            return game;
        }

        public static ParsedGame BusinessFullToWebParsedGame(Business.FullGame fullGame)
        {
            ParsedGame game = new ParsedGame();
            game.game = BusinessToWebGame(fullGame.game);
            game.genres = fullGame.genres;
            game.platforms = fullGame.platforms;

            return game;
        }

        public static Genre BusinessToWebGenre(Business.Genre businessGenre)
        {
            Genre genre = new Genre();
            genre.Id = businessGenre.Id;
            genre.Name = businessGenre.Name;
            genre.ParentGenreId = businessGenre.ParentGenreId;

            return genre;
        }

        public static ShortGenre BusinessToWebShortGenre(Business.Genre businessGenre)
        {
            ShortGenre genre = new ShortGenre();
            genre.Id = businessGenre.Id;
            genre.Name = businessGenre.Name;

            return genre;
        }

        public static Platform BusinessToWebPlatform(Business.Platform businessPlatform)
        {
            Platform platform = new Platform();
            platform.Id = businessPlatform.Id;
            platform.Type = businessPlatform.Type;

            return platform;
        }
    }
}
