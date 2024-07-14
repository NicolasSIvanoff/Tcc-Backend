using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TccBackend.DTOs;
using TccBackend.Models;
using TccBackend.Services;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;


namespace TccBackend.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ITokenService tokenService,
                              UserManager<ApplicationUser> userManager,
                              RoleManager<IdentityRole> roleManager,
                              IConfiguration configuration,
                              ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Route("createRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (roleResult.Succeeded)
                {
                    _logger.LogInformation(1, "Roles Added");

                    return StatusCode(StatusCodes.Status200OK,
                        new Response
                        {
                            Status = "Success",
                            Message =
                            $"Role {roleName} created successfully!"
                        });
                }
                else
                {

                   _logger.LogInformation(2,"Error");
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response
                        {
                            Status = "Error",
                            Message = "Role creation failed! Please check role details and try again."
                        });
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                new Response
                {
                    Status = "Error",
                    Message = "Role already exists!!"
                });
        }  
        
        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByNameAsync(email);
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, $"User {user.Email} Added to the {roleName} Role");
                    return StatusCode(StatusCodes.Status200OK,
                        new Response
                        {
                            Status = "Success",
                            Message = $"User {user.Email} added to role {roleName} successfully!"
                        });
                }
                else
                {

                   _logger.LogInformation(1, $"Error: Unable ton add user {user.Email} to the {roleName} role");
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response
                        {
                            Status = "Error",
                            Message = "User addition to role failed! Please check user details and try again."
                        });
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                new Response
                {
                    Status = "Error",
                    Message = "User does not exist!"
                });
          
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Name);

            if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password!))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = _tokenService.GenerateAccessToken(authClaims,
                                                              _configuration);

                var refreshToken = _tokenService.GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"],
                                 out int refreshTokenValidityInMinutes);
                user.RefreshTokenExpiryTime =
                                 DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);
                user.RefreshToken = refreshToken;

                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]

        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (model == null)
            {
                return BadRequest(new Response { Status = "Error", Message = "Invalid user data." });
            }

            var userExists = await _userManager.FindByNameAsync(model.Name);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "User already exists!" });
            }

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Name
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = $"User creation failed! {errors}" });
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("refresh-token")]

        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = tokenModel.AccessToken ??
                    throw new ArgumentException(nameof(tokenModel));

            string? refreshToken = tokenModel.RefreshToken ??
                throw new ArgumentException(nameof(tokenModel));

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

            if (principal == null)
            {
                return BadRequest("Invalid access token/refresh token");
            }
            string username = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(username!);
            if (user == null || user.RefreshToken != refreshToken
                             || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token/refresh token");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(
                                                principal.Claims.ToList(), _configuration);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken,
            });
        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]

        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByIdAsync(username);
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return NoContent();
        }
    }
}
