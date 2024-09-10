using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GameStore.Controllers;
using GameStore.Models;
using Business = GameStore.BusinessLogic.Models;
using Entity = GameStore.DataAccess.Models;
using GameStore.BusinessLogic.Repositories;
using GameStore.DataAccess.Repositories;
using System.IO;


namespace GameStore.Controllers
{
    public class GamesController : Controller
    {
        private readonly ILogger<GamesController> _logger;
        private readonly IGameRepository _gameRepository;

        public GamesController(IGameRepository gameRepository, ILogger<GamesController> logger)
        {
            _gameRepository = gameRepository;
            _logger = logger;
        }


        [HttpPost("games")]
        public ActionResult<Guid> AddNewGame([FromBody] ParsedGame parsedGame)
        {
            WebToBusiness.CheckParsedGame(parsedGame);

            var businessGame = WebToBusiness.WebToBusinessGame(parsedGame);
            return _gameRepository.AddNewGame(businessGame);
        }



        [HttpGet("games/{key}")]
        public ActionResult<Game> GetGameByKey(string key)
        {
            WebToBusiness.CheckAndThrowExceptionString(key, "Incorrect Key");

            return BusinessToWeb.BusinessToWebGame(_gameRepository.GetGameByKey(key));
        }



        [HttpGet("games/find/{id}")]
        public ActionResult<Game> GetGameById(Guid id)
        {
            WebToBusiness.CheckAndThrowExceptionGuid(id);

            return BusinessToWeb.BusinessToWebGame(_gameRepository.GetGameByGameId(id));
        }



        [HttpPut("games")]
        public ActionResult<Guid> UpdateGame([FromBody] ParsedGame parsedGame)
        {
            WebToBusiness.CheckAndThrowExceptionGuid(parsedGame.game.Id);
            WebToBusiness.CheckParsedGame(parsedGame);

            var businessGame = WebToBusiness.WebToBusinessGame(parsedGame);
            return _gameRepository.UpdateGame(businessGame);
        }



        [HttpDelete("/games/{key}")]
        public IActionResult DeleteGame(string key)
        {
            WebToBusiness.CheckAndThrowExceptionString(key, "Incorrect Key");
            
            _gameRepository.DeleteGame(key);
            return Ok();
        }



        [HttpGet("/games/{key}/file")]
        public IActionResult DownloadGameFile(string key)
        {
            WebToBusiness.CheckAndThrowExceptionString(key, "Incorrect Key");

            ParsedGame game = BusinessToWeb.BusinessFullToWebParsedGame(_gameRepository.GetFullGame(_gameRepository.GetGameByKey(key).Id));

            string result = JsonSerializer.Serialize(game);

            byte[] byteArray = Encoding.Unicode.GetBytes(result);
            MemoryStream stream = new MemoryStream(byteArray);

            return File(stream, "text/plain", $"<{game.game.Name}>_<{DateTime.Now}>.txt");
        }



        [HttpGet("games")]
        public ActionResult<List<Game>> GetAllGames()
        {
            List<Business.Game> businessGames = _gameRepository.GetAllGames();
            List<Game> result = new List<Game>();

            foreach (var item in businessGames)
            {
                result.Add(BusinessToWeb.BusinessToWebGame(item));
            }

            return result;
        }



        [HttpGet("/games/{key}/genres")]
        public ActionResult<List<ShortGenre>> GetGenresByGameKey(string key)
        {
            WebToBusiness.CheckAndThrowExceptionString(key, "Incorrect Key");

            var game = _gameRepository.GetGameByKey(key);
            var entityGenres = _gameRepository.GetGenresByGameId(game.Id);
            List<ShortGenre> result = new List<ShortGenre>();

            foreach (var item in entityGenres)
            {
                result.Add(BusinessToWeb.BusinessToWebShortGenre(item));
            }

            return result;
        }



        [HttpGet("/games/{key}/platforms")]
        public ActionResult<List<Platform>> GetPlatformsByGameKey(string key)
        {
            WebToBusiness.CheckAndThrowExceptionString(key, "Incorrect Key");

            var gameId = _gameRepository.GetGameByKey(key).Id;
            var entityPlatforms = _gameRepository.GetPlatformsByGameId(gameId);
            List<Platform> result = new List<Platform>();

            foreach (var item in entityPlatforms)
            {
                result.Add(BusinessToWeb.BusinessToWebPlatform(item));
            }

            return result;
        }






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }


}
