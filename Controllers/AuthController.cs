using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using CQRSExample.Features.Auth.Commands;
using CQRSExample.Features.Auth.Queries;
using CQRSExample.Features.Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;

namespace CQRSExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(new { Message = "Registration successful" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserQuery query)
        {
            var result = await _mediator.Send(query);
            if (!result.Succeeded)
            {
                return Unauthorized(result.Errors);
            }

            // Set refresh token in HTTP-only cookie
            Response.Cookies.Append("refreshToken", result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Set to true in production for HTTPS
                   // SameSite = SameSiteMode.Strict,
                    SameSite = SameSiteMode.None, // For cross-site cookies
                    Expires = DateTime.UtcNow.AddDays(result.RefreshTokenExpirationDays)
                });

            return Ok(new { result.AccessToken });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new { Message = "Refresh token not found" });
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var command = new RefreshAccessTokenCommand(refreshToken, ipAddress);
            var result = await _mediator.Send(command);

            if (!result.Succeeded)
            {
                return Unauthorized(result.Errors);
            }

            // Set new refresh token in HTTP-only cookie
            Response.Cookies.Append("refreshToken", result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Set to true in production for HTTPS
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(result.RefreshTokenExpirationDays)
                });

            return Ok(new { result.AccessToken });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (!string.IsNullOrEmpty(refreshToken))
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                var command = new RevokeRefreshTokenCommand(refreshToken, ipAddress);
                await _mediator.Send(command);
            }

            Response.Cookies.Delete("refreshToken");

            return Ok(new { Message = "Logout successful" });
        }
    }
}
