using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NCD.Core.Messages.Integration;
using NCD.Identity.API.DTO;
using NCD.MessageBus;
using System;
using System.Threading.Tasks;

namespace NCD.Identity.API.Controllers
{
    [Route("api/identity")]
    public class IdentityController : Controller
    {
        public readonly SignInManager<IdentityUser> _signInManager;
        public readonly UserManager<IdentityUser> _userManager;
        private readonly IMessageBus _messageBus;

        public IdentityController(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IMessageBus messageBus)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _messageBus = messageBus;
        }

        [HttpPost("new-user")]
        public async Task<ActionResult> Register([FromBody]NewUserDTO newUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new IdentityUser
            {
                UserName = newUser.Email,
                Email = newUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, newUser.Password);

            if (result.Succeeded)
            {
                var customerResult = await RegisterCustomer(newUser);

                if (!customerResult.ValidationResult.IsValid)
                {
                    await DeleteUser(user);
                    return BadRequest(customerResult.ValidationResult.Errors);
                }

                return Ok("Created");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDTO login)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _signInManager
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

        private async Task<ResponseMessage> RegisterCustomer(NewUserDTO newUser)
        {
            var user = await _userManager.FindByEmailAsync(newUser.Email);

            var registeredUser = new RegisteredUserIntegrationEvent(
                                               id: Guid.Parse(user.Id),
                                               name: newUser.Name,
                                               email: newUser.Email,
                                               document: newUser.Document);
            try
            {
                return await _messageBus.RequestAsync<RegisteredUserIntegrationEvent, ResponseMessage>(registeredUser);
            }
            catch
            {
                await DeleteUser(user);
                throw;
            }
        }

        private async Task DeleteUser(IdentityUser user)
        {
            await _userManager.DeleteAsync(user);
            var userdeletedEvent = new UserDeletedIntegrationEvent(Guid.Parse(user.Id));
            await _messageBus.PublishAsync(userdeletedEvent);
        }
    }
}
