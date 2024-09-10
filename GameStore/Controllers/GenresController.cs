using GameStore.BusinessLogic.Repositories;
using Business = GameStore.BusinessLogic.Models;
using GameStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers
{
    public class GenresController : Controller
    {
        private readonly ILogger<GenresController> _logger;
        private readonly IGameRepository _gameRepository;

        public GenresController(IGameRepository gameRepository, ILogger<GenresController> logger)
        {
            _gameRepository = gameRepository;
            _logger = logger;
        }


        [HttpGet("genres/{id}/games")]
        public ActionResult<List<Game>> GetGamesByGenreId(Guid id)
        {
            WebToBusiness.CheckAndThrowExceptionGuid(id);

            List<Game> games = new List<Game>();
            List<Business.Game> businessList = _gameRepository.GetGamesByGenreId(id);
            foreach (var item in businessList)
            {
                games.Add(BusinessToWeb.BusinessToWebGame(item));
            }

            return games;
        }




        [HttpPost("genres")]
        public ActionResult<Guid> AddNewGenre([FromBody] ParsedGenre genre)
        {
            WebToBusiness.CheckAndThrowExceptionString(genre.genre.Name, "Incorrect Genre Name");
            WebToBusiness.CheckAndCreateNewGuid(genre.genre.Id);

            return _gameRepository.AddNewGenre(WebToBusiness.WebToBusinessGenre(genre.genre));
        }



        [HttpGet("/genres/{Id}")]
        public ActionResult<Genre> GetGenre(Guid id)
        {
            WebToBusiness.CheckAndThrowExceptionGuid(id);

            return BusinessToWeb.BusinessToWebGenre(_gameRepository.GetGenreById(id));
        }



        [HttpGet("/genres")]
        public ActionResult<List<ShortGenre>> GetAllGenres()
        {
            List<ShortGenre> genres = new List<ShortGenre>();

            var businessGenres = _gameRepository.GetAllGenres();

            foreach (var item in businessGenres)
            {
                genres.Add(BusinessToWeb.BusinessToWebShortGenre(item));
            }

            return genres;
        }



        [HttpGet("/genres/{id}/genres")]
        public ActionResult<List<ShortGenre>> GetGenresByParentId(Guid id)
        {
            WebToBusiness.CheckAndThrowExceptionGuid(id);

            var businessList = _gameRepository.GetGenresByParentId(id);
            List<ShortGenre> genres = new List<ShortGenre>();

            foreach (var item in businessList)
            {
                genres.Add(BusinessToWeb.BusinessToWebShortGenre(item));
            }

            return genres;
        }



        [HttpPut("genres")]
        public ActionResult<Guid> UpdateGenre([FromBody] ParsedGenre genre)
        {
            WebToBusiness.CheckAndThrowExceptionGuid(genre.genre.Id);
            WebToBusiness.CheckAndThrowExceptionString(genre.genre.Name, "Incorrect Genre Name");

            return _gameRepository.UpdateGenre(WebToBusiness.WebToBusinessGenre(genre.genre));
        }



        [HttpDelete("/genres/{Id}")]
        public ActionResult<Guid> DeleteGenre(Guid id)
        {
            WebToBusiness.CheckAndThrowExceptionGuid(id);

            return _gameRepository.DeleteGenre(id);
        }

    }
}
