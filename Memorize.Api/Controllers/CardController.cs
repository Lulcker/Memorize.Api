using Memorize.Api.Models.Entities;
using Memorize.Api.Repositories;
using Memorize.Library.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Memorize.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/card")]
    public class CardController : Controller
    {
        private readonly ICardRepository _cardRepository;
        private static IConfigurationRoot MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        public CardController(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        /// <summary>
        /// Метод добавления карточки в коллекцию
        /// </summary>
        /// <param name="request">Форма запроса</param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddCard([FromBody] CardRequest request)
        {
            request.UserName = User.FindFirst(MyConfig.GetValue<string>("ClaimEmail")!)!.Value;
            var result = await _cardRepository.AddCard(request);

            if (result.Item1)
                return Ok(result.Item2);

            return BadRequest(result.Item2);
        }

        /// <summary>
        /// Метод обновления карточки 
        /// </summary>
        /// <param name="request">Форма запроса</param>
        /// <returns></returns>
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateCard([FromBody] CardRequest request)
        {
            var result = await _cardRepository.UpdateCard(request);

            if (result.Item1) 
                return Ok(result.Item2);

            return BadRequest(result.Item2);
        }

        /// <summary>
        /// Метод удаления карточки из коллекции
        /// </summary>
        /// <param name="cardId">Id карточки</param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCard([FromQuery] int cardId)
        {
            var result = await _cardRepository.DeleteCard(cardId);

            if (result.Item1)
                return Ok(result.Item2);

            return BadRequest(result.Item2);
        }

        /// <summary>
        /// Метод вывода всех карточек в коллекции
        /// </summary>
        /// <param name="collectionCardId">Id коллекции карточек</param>
        /// <returns></returns>
        [HttpGet("all-cards-by-collection")]
        public async Task<IActionResult> AllCardsByCollection([FromQuery] int collectionCardId)
        {
            var result = await _cardRepository.AllCardsByCollection(collectionCardId);

            return Json(result);
        }
    }
}
