using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NCD.Identity.API.DTO;
using System.Threading.Tasks;

namespace NCD.Identity.API.Controllers
{
    [Route("api/identity")]
    public class IdentityController : Controller
    {
        public readonly SignInManager<IdentityUser> SignInManager;
        public readonly UserManager<IdentityUser> UserManager;

        public IdentityController(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            SignInManager = signInManager;
            UserManager = userManager;
        }

        [HttpPost("new-user")]
        public async Task<ActionResult> Registrar(NewUserDTO newUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new IdentityUser
            {
                UserName = newUser.Email,
                Email = newUser.Email,
                EmailConfirmed = true
            };

            var result = await UserManager.CreateAsync(user, newUser.Password);

            if(result.Succeeded) return Ok("Created");

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDTO login)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await SignInManager
                               .PasswordSignInAsync(login.Email, 
                                                    login.Password,
                                                    false, 
                                                    true);

            if (result.Succeeded)
            {
                return Ok("user is valid");
            }

            if (result.IsLockedOut)
            {
                return BadRequest("blocked");
            }
            
            return BadRequest("other errors");
        }
    }
}
