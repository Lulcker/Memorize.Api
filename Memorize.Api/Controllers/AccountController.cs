using Memorize.Library.Request;
using Microsoft.AspNetCore.Mvc;
using Memorize.Api.Repositories;

namespace Memorize.Api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// Метод регистрации
        /// </summary>
        /// <param name="request">Форма запроса</param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.Register(request);

                if (result.Item1)
                    return Ok(result.Item2);

                return BadRequest(result.Item2);

            }
            return BadRequest("Введены не все данные");
        }

        /// <summary>
        /// Метод входа в аккаунт
        /// </summary>
        /// <param name="request">Форма запроса</param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.Login(request);

                if (result.Item1) 
                    return Ok(result.Item2);

                return BadRequest(result.Item2);
            }

            return BadRequest("Введены не все данные");
        }
    }
}
