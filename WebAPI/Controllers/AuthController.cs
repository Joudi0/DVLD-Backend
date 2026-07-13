using BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using WebAPI.Services;
using Shared;
using System;

namespace WebAPI.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting(Shared.clsProjectPolicies.AuthPolicy)]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        /// <summary>
        /// Authenticates a user and returns separate short-lived Access Token and secure Refresh Token.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDto, [FromServices] clsTokenService tokenService)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Username) || string.IsNullOrEmpty(loginDto.Password))
                return BadRequest("Username and Password are required.");

            var authData = await clsUser.checkLogin(loginDto.Username, loginDto.Password);
            if (authData == null) 
                return Unauthorized("Invalid username or password.");

            string accessToken = tokenService.GenerateAccessToken(authData.UserID, loginDto.Username, authData.UserRoleID.ToString());
            string refreshToken = await tokenService.GenerateAndSaveRefreshTokenAsync(authData.UserID);

            return Ok(new TokenResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        /// <summary>
        /// Registers a new user with automated password hashing and salting.
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] Shared.RegisterRequestDTO registerDto)
        {
            if (registerDto == null || string.IsNullOrEmpty(registerDto.UserName) || string.IsNullOrEmpty(registerDto.Password))
                return BadRequest("Invalid registration data. UserName and Password are required.");

            try
            {
                int insertedId = await clsUser.RegisterUser(registerDto);
                
                if (insertedId == -1)
                    return BadRequest("Registration failed. Could not create the user.");

                return Ok(new { Id = insertedId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Rotates the refresh token and grants a new access token securely using original signatures.
        /// </summary>
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(TokenResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDTO request, [FromServices] clsTokenService tokenService)
        {
            if (request == null || string.IsNullOrEmpty(request.RefreshToken))
                return BadRequest("Invalid client request. RefreshToken is required.");

            int isValidToken = await tokenService.ValidateAndRevokeRefreshTokenAsync(request.UserID, request.RefreshToken);
            if (isValidToken == -1)
                return Unauthorized("Invalid or expired refresh token.");

            string newAccessToken = tokenService.GenerateAccessToken(request.UserID, request.Username, Shared.enRoles.User.ToString());
            string newRefreshToken = await tokenService.GenerateAndSaveRefreshTokenAsync(request.UserID);

            return Ok(new TokenResponseDTO
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        /// <summary>
        /// Revokes the refresh token inside the database securely using original signatures.
        /// </summary>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDTO request, [FromServices] clsTokenService tokenService)
        {
            if (request == null || string.IsNullOrEmpty(request.RefreshToken))
                return Ok(); 

            await tokenService.RevokeTokenByRawAsync(request.UserID, request.RefreshToken);
            return Ok("Logged out successfully.");
        }

    }
}