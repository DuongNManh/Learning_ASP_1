using Learning_Web.API.Models.DTO;
using Learning_Web.API.Models.Response;
using Learning_Web.API.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Learning_Web.API.Exceptions;

namespace Learning_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
            _logger = logger;
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new BadRequestException("Invalid registration data");
                }

                var identityUser = new IdentityUser
                {
                    UserName = registerDTO.Username,
                    Email = registerDTO.Username
                };

                var result = await _userManager.CreateAsync(identityUser, registerDTO.Password);
                if (!result.Succeeded)
                {
                    throw new BadRequestException(string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                if (registerDTO.Roles?.Any() == true)
                {
                    var roleResult = await _userManager.AddToRolesAsync(identityUser, registerDTO.Roles);
                    if (!roleResult.Succeeded)
                    {
                        throw new ApiException("User was registered but roles could not be assigned", HttpStatusCode.PartialContent);
                    }
                }

                return Ok("User was registered successfully");
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during user registration");
                throw new ApiException("Registration failed", HttpStatusCode.InternalServerError);
            }
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new BadRequestException("Invalid login credentials");
                }

                var user = await _userManager.FindByEmailAsync(loginDTO.Username);
                if (user == null)
                {
                    throw new UnauthorizedException("Invalid credentials");
                }

                var validPassword = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
                if (!validPassword)
                {
                    throw new UnauthorizedException("Invalid credentials");
                }

                var roles = await _userManager.GetRolesAsync(user);
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
            catch (UnauthorizedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login");
                throw new ApiException("Login failed", HttpStatusCode.InternalServerError);
            }
        }

    }
}
