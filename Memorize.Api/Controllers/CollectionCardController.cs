using Memorize.Api.Models.Entities;
using Memorize.Api.Repositories;
using Memorize.Library.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Memorize.Api.Controllers
{
    /// <summary>
    /// Контроллер коллекций карт
    /// </summary>
    [Route("api/collection")]
    [ApiController]
    [Authorize]
    public class CollectionCardController : Controller
    {
        private readonly ICollectionCardRepository _collectionCardRepository;
        private static IConfigurationRoot MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        public CollectionCardController(ICollectionCardRepository collectionCardRepository)
        {
            _collectionCardRepository = collectionCardRepository;
        }

        /// <summary>
        /// Метод добавления коллекции
        /// </summary>
        /// <param name="name">Форма запроса</param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddCollectionCard([FromBody] string name)
        {
            var result = await _collectionCardRepository.AddCollectionCard(new CollectionCard { Name = name, UserName = User.FindFirst(MyConfig.GetValue<string>("ClaimEmail")!)!.Value });

            if (result.Item1)
                return Ok(result.Item2);
            
            return BadRequest(result.Item2);
        }

        /// <summary>
        /// Метод обновления названия коллекции
        /// </summary>
        /// <param name="request">Форма запроса</param>
        /// <returns></returns>
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateCollectionCard([FromBody] UpdateCollectionCardRequest request)
        {
            request.UserName = User.FindFirst(MyConfig.GetValue<string>("ClaimEmail")!)!.Value;
            var result = await _collectionCardRepository.UpdateCollectionCard(request);

            if (result.Item1)
                return Ok(result.Item2);

            return BadRequest(result.Item2);
        }

        /// <summary>
        /// Метод удаления коллекции
        /// </summary>
        /// <param name="collectionCardId">Id коллекции карточек</param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCollectionCard([FromQuery] int collectionCardId)
        {
            var result = await _collectionCardRepository.DeleteCollectionCard(new CollectionCard { Id = collectionCardId, UserName = User.FindFirst(MyConfig.GetValue<string>("ClaimEmail")!)!.Value });

            if (result.Item1)
                return Ok(result.Item2);

            return BadRequest(result.Item2);
        }

        /// <summary>
        /// Метод вывода всех коллекций карточек для пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet("all-collection-cards-by-user")]
        public async Task<IActionResult> AllCollectionCardsByUser()
        {
            var result = await _collectionCardRepository.AllCollectionCardsByUser(User.FindFirst(MyConfig.GetValue<string>("ClaimEmail")!)!.Value);

            return Json(result);
        }
    }
}
