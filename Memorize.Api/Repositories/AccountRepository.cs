using Memorize.Library.Request;
using Memorize.Library.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Memorize.Api.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private static IConfigurationRoot MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        public AccountRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<(bool, AuthResponse)> Register(RegistrationRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Name);

            if (user != null)
                return (false, new AuthResponse() { Errors = new List<string>() { "Пользователь с такой почтой уже зарегистрирован" } });

            var newUser = new IdentityUser() { Email = request.Name, UserName = request.Email };

            var isCreated = await _userManager.CreateAsync(newUser, request.Password);
            if (isCreated.Succeeded)
            {
                var jwtToken = GenerateJwtToken(newUser);

                return (true, new AuthResponse() { Token = jwtToken });
            }
            else
            {
                Dictionary<string, string> errorsDict = new Dictionary<string, string>()
                    {
                        { "Passwords must be at least 8 characters.", "Пароль должен содержать не менее 8 символов" },
                        { "Passwords must have at least one digit ('0'-'9').", "Пароль должен содержать по крайней мере одну цифру" },
                        { "Passwords must have at least one uppercase ('A'-'Z').", "Пароль должен содержать по крайней мере одну заглавную букву" }
                    };

                List<string> errors = new List<string>();
                foreach (var error in isCreated.Errors.Select(x => x.Description).ToList())
                {
                    if (errorsDict.ContainsKey(error))
                        errors.Add(errorsDict[error]);
                }

                return (false, new AuthResponse() { Errors = errors });
            }
        }

        public async Task<(bool, AuthResponse)> Login(UserLoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return (false, new AuthResponse() { Errors = new List<string>() { "Пользователя с такой почтой не существует" } });

            var isCorrect = await _userManager.CheckPasswordAsync(user, request.Password);

            if (isCorrect)
            {
                var jwtToken = GenerateJwtToken(user);

                return (true, new AuthResponse() { Token = jwtToken });
            }
            else
                return (false, new AuthResponse() { Errors = new List<string>(){ "Почта или пароль введены неправильно" } });
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(MyConfig.GetValue<string>("JWTKey")!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddMonths(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
