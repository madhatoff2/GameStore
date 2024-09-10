using GameStore.Exceptions;
using GameStore.Models;
using Microsoft.IdentityModel.Tokens;
using Business = GameStore.BusinessLogic.Models;

namespace GameStore.Controllers
{
    public class WebToBusiness
    {
        public static Business.FullGame WebToBusinessGame(ParsedGame parsedGame)
        {
            Business.Game tempGame = new Business.Game();
            tempGame.Id = (Guid)parsedGame.game.Id;
            tempGame.Key = parsedGame.game.Key;
            tempGame.Name = parsedGame.game.Name;
            tempGame.Description = parsedGame.game.Description;

            Business.FullGame businessGame = new Business.FullGame();
            businessGame.game = tempGame;

            foreach (var item in parsedGame.genres)
            {
                businessGame.genres.Add(item);
            }

            foreach (var item in parsedGame.platforms)
            {
                businessGame.platforms.Add(item);
            }

            return businessGame;
        }

        public static Business.Genre WebToBusinessGenre(Genre genre)
        {
            Business.Genre businessGenre = new Business.Genre();
            businessGenre.Id = (Guid)genre.Id!;
            businessGenre.Name = genre.Name!;
            businessGenre.ParentGenreId = genre.ParentGenreId;

            return businessGenre;
        }

        public static Business.Platform WebToBusinessPlatform(Platform platform)
        {
            Business.Platform businessPlatform = new Business.Platform();
            businessPlatform.Id = (Guid)platform.Id!;
            businessPlatform.Type = platform.Type!;

            return businessPlatform;
        }

        public static Guid? CheckAndCreateNewGuid(Guid? id)
        {
            if (id == Guid.Empty || id == null || id.ToString().Length < 36)
            {
                id = Guid.NewGuid();
            }

            return id;
        }

        public static void CheckAndThrowExceptionGuid(Guid? id)
        {
            if (id == Guid.Empty || id == null || id?.ToString().Length < 36)
            {
                throw new ArgumentException("Guid is incorrect");
                
            }
        }

        public static string CheckAndCreateNewString(string? key, string name)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                key = name + "_Key";
            }

            return key;
        }

        public static void CheckAndThrowExceptionString(string? name, string errorMessage = "Incorrect string value")
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(errorMessage);
            }
        }

        public static void CheckParsedGame(ParsedGame parsedGame)
        {
            CheckAndThrowExceptionString(parsedGame.game.Name, "Incorrect Game Name");
            parsedGame.game.Id = CheckAndCreateNewGuid(parsedGame.game.Id);
            parsedGame.game.Key = CheckAndCreateNewString(parsedGame.game.Key, parsedGame.game.Name);

            foreach (var genre in parsedGame.genres) { CheckAndThrowExceptionGuid(genre); }
            foreach (var platform in parsedGame.platforms) { CheckAndThrowExceptionGuid(platform); }
        }

    }
}
