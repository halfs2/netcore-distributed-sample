using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NCD.Core.Messages.Integration;
using NCD.Identity.API.DTO;
using NCD.Identity.API.Services;
using NCD.MessageBus;
using System;
using System.Threading.Tasks;

namespace NCD.Identity.API.Controllers
{
    [Route("api/identity")]
    public class IdentityController : Controller
    {
        private readonly IMessageBus _messageBus;
        private readonly AuthenticationService _authenticationService;

        public IdentityController(
            AuthenticationService authenticationService,
            IMessageBus messageBus)
        {
            _messageBus = messageBus;
            _authenticationService = authenticationService;
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

            var result = await _authenticationService.UserManager.CreateAsync(user, newUser.Password);

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

            var result = await _authenticationService.SignInManager
                               .PasswordSignInAsync(login.Email, 
                                                    login.Password,
                                                    false, 
                                                    true);

            if (result.Succeeded)
            {
                return Ok(await _authenticationService.GenerateJwt(login.Email));
            }

            if (result.IsLockedOut)
            {
                return BadRequest("blocked");
            }
            
            return BadRequest("other errors");
        }
        
        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken)) return BadRequest("Invalid Refresh token");
           
            var token = await _authenticationService.GetRefreshToken(Guid.Parse(refreshToken));

            if (token is null) return BadRequest("Refresh token expired");
            
            return Ok(await _authenticationService.GenerateJwt(token.Username));
        }

        private async Task<ResponseMessage> RegisterCustomer(NewUserDTO newUser)
        {
            var user = await _authenticationService.UserManager.FindByEmailAsync(newUser.Email);

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
            await _authenticationService.UserManager.DeleteAsync(user);
            var userdeletedEvent = new UserDeletedIntegrationEvent(Guid.Parse(user.Id));
            await _messageBus.PublishAsync(userdeletedEvent);
        }
    }
}
