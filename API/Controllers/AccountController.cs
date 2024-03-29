﻿using API.DTOs;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JWTService _jWTService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(JWTService jWTService,
            SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            _jWTService = jWTService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet("refresh-token")]
        public async Task<ActionResult<UserDto>> RefreshToken()
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.Email)?.Value);
            return Ok(CreateApplicationUserDto(user));
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.UserName);

            if (user is null)
            {
                return Unauthorized("Invalid username or password");
            }

            if (user.EmailConfirmed == false)
            {
                return Unauthorized("You must confirm your email before logging in");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized("Invalid username or password");
            }

            return CreateApplicationUserDto(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerModel)
        {
            if (await CheckEmailExistsAsync(registerModel.Email))
            {
                return BadRequest($"An existing account is using {registerModel.Email} email adress. " +
                    $"Please try with another email adress");
            }

            var userToAdd = new User()
            {
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Email = registerModel.Email,
                UserName = registerModel.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(userToAdd, registerModel.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new JsonResult(new { title = "Account created", message = "Your account has been successfully created!" }));
        }

        #region Private Helper Methods
        private UserDto CreateApplicationUserDto(User user)
        {
            return new UserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                JWT = _jWTService.GenerateJWT(user)
            };
        }

        private async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.Users.AnyAsync(user => user.Email == email.ToLower());
        }
        #endregion

    }
}
