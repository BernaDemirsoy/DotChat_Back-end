using AutoMapper;
using DotChat_DTOs.Account;
using DotChat_Entities.DbSet;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DotChat_WebApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IMapper mapper;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> login(LoginDto logindto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User user = await userManager.FindByEmailAsync(logindto.Email);
            if (user == null)
            {
                return NotFound(user);
            }
            var isValidPassword = await signInManager.CheckPasswordSignInAsync(user, logindto.PasswordHash, false);
            if (!isValidPassword.Succeeded)
                return Unauthorized();
            return Ok(user);
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> register(NewUserDto newUser)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (newUser.confirmedPasswordHash != newUser.passwordHash)
            {
                return BadRequest();
            }
            User user = new();
            mapper.Map(newUser, user);
            var result = await userManager.CreateAsync(user, user.PasswordHash);
            var allResult = new
            {
                user = user,
                result = result
            };
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(allResult);
        }
        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<IActionResult> setAvatar(string id, [FromBody]SetAvatarDto avatarInfo)
        {
            try
            {
               
                User exacUser = await userManager.FindByIdAsync(id);
                if (exacUser is null)
                {
                    return BadRequest("We could not find this user.Sorry, please try again.");
                }
                mapper.Map(avatarInfo, exacUser);
                var result=await userManager.UpdateAsync(exacUser);
                var allResult = new
                {
                    user = exacUser,
                    result = result
                };
                if (!result.Succeeded)
                    return BadRequest(result);
                return Ok(allResult);
            }
            catch (Exception)
            {

                throw ;
            }
        }
    }
}