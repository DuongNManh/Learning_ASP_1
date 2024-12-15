using Learning_Web.API.Models.DTO;
using Learning_Web.API.Models.Response;
using Learning_Web.API.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            // validate the DTO
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            var identityUser = new IdentityUser
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Username
            };

            var result = await _userManager.CreateAsync(identityUser, registerDTO.Password);

            if (!result.Succeeded)
            {
                return BadRequest("User was not registered!");
            }

            if (registerDTO.Roles != null && registerDTO.Roles.Any())
            {
                var roleResult = await _userManager.AddToRolesAsync(identityUser, registerDTO.Roles);
                if (!roleResult.Succeeded)
                {
                    return BadRequest("User was registered but roles were not assigned!");
                }
            }

            return Ok("User was registered! Please Login.");
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            // validate the DTO
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            var user = await _userManager.FindByEmailAsync(loginDTO.Username);

            if (user != null)
            {
                var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
                if (result)
                {
                    // get roles of the user
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        // generate token
                        var token = await _tokenRepository.GenerateToken(user, roles.ToList());

                        // return the response with token
                        LoginResponse response = new LoginResponse
                        {
                            Token = token,
                            RefreshToken = "",
                            UserName = user.UserName,
                            Email = user.Email,
                            Roles = roles.ToArray()
                        };

                        return Ok(response);
                    }
                }
            }
            return BadRequest("User name or password incorrect!");
        }

    }
}
