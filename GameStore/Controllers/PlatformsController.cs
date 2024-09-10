using Microsoft.AspNetCore.Mvc;
using GameStore.Models;
using GameStore.BusinessLogic.Repositories;
using Business = GameStore.BusinessLogic.Models;

namespace GameStore.Controllers
{
    public class PlatformsController : Controller
    {
        private readonly ILogger<PlatformsController> _logger;
        private readonly IGameRepository _gameRepository;

        public PlatformsController(IGameRepository gameRepository, ILogger<PlatformsController> logger)
        {
            _gameRepository = gameRepository;
            _logger = logger;
        }



        [HttpPost("platforms")]
        public ActionResult<Guid> CreatePlatform([FromBody] ParsedPlatform platform)
        {
            WebToBusiness.CheckAndCreateNewGuid(platform.platform.Id);
            WebToBusiness.CheckAndThrowExceptionString(platform.platform.Type, "Incorrect Platform Type");

            Business.Platform businessPlatform = WebToBusiness.WebToBusinessPlatform(platform.platform);
            return _gameRepository.CreatePlatform(businessPlatform);
        }


        [HttpGet("platforms/{id}/games")]
        public ActionResult<List<Game>> GetGamesByPlatform(Guid id)
        {
            WebToBusiness.CheckAndThrowExceptionGuid(id);

            List<Game> games = new List<Game>();
            List<Business.Game> businessList = _gameRepository.GetGamesByPlatformId(id);

            foreach (var item in businessList)
            {
                games.Add(BusinessToWeb.BusinessToWebGame(item));
            }

            return games;
        }


        [HttpGet("/platforms/{Id}")]
        public ActionResult<Platform> GetPlatformById(Guid id)
        {
            WebToBusiness.CheckAndThrowExceptionGuid(id);

            return BusinessToWeb.BusinessToWebPlatform(_gameRepository.GetPlatformById(id));
        }


        [HttpGet("platforms")]
        public ActionResult<List<Platform>> GetAllPlatforms()
        {
            var list = _gameRepository.GetAllPlatforms();
            List<Platform> platforms = new List<Platform>();

            foreach (var item in list)
            {
                platforms.Add(BusinessToWeb.BusinessToWebPlatform(item));
            }

            return platforms;
        }


        [HttpPut("platforms")]
        public ActionResult<Guid> UpdatePlatform([FromBody] ParsedPlatform platform)
        {
            WebToBusiness.CheckAndThrowExceptionGuid(platform.platform.Id);
            WebToBusiness.CheckAndThrowExceptionString(platform.platform.Type, "Incorrect Platform Type");

            var businessPlatform = WebToBusiness.WebToBusinessPlatform(platform.platform);
            _gameRepository.UpdatePlatform(businessPlatform);

            return businessPlatform.Id;
        }


        [HttpDelete("platforms")]
        public IActionResult DeletePlatform(Guid id)
        {
            WebToBusiness.CheckAndThrowExceptionGuid(id);

            _gameRepository.DeletePlatform(id);

            return Ok();
        }

    }
}
