using System;
using Blog.Api.Services;
using Blog.Api.ViewModels.Auth;
using Blog.Data.Repositories;
using Blog.Model.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public AuthController(IAuthService authService, IUserRepository userRepository) {
            _authService = authService;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public ActionResult<AuthData> Login([FromBody] LoginViewModel model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = _userRepository.GetSingle(x => x.Email == model.Email);
            if (user == null) {
                return BadRequest(new { email = "no user with this email" });
            }

            var passwordValid = _authService.VerifyPassword(model.Password, user.Password);
            if (!passwordValid) {
                return BadRequest(new { password = "invalid password" });
            }

            return _authService.GetAuthData(user.Id);
        }

        [HttpPost("register")]
        public ActionResult<AuthData> Register([FromBody] RegisterViewModel model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var emailUniq = _userRepository.isEmailUniq(model.Email);
            if (!emailUniq) {
                return BadRequest(new { email = "user with this email already exists" });
            }

            var usernameUniq = _userRepository.IsUsernameUniq(model.Username);
            if (!usernameUniq) {
                return BadRequest(new { username = "user with this email already exists" });
            }

            var id = Guid.NewGuid().ToString();
            var user = new User() {
                Id = id,
                Username = model.Username,
                Email = model.Email,
                Password = _authService.HashPassword(model.Password)
            };
            _userRepository.Add(user);
            _userRepository.Commit();

            return _authService.GetAuthData(user.Id);
        }
    }
}
