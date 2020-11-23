using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApiUtilities.Abstract;

namespace WebApiUtilities.Identity
{
    public class UserController : UnsecuredController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_userService.GetAll());
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] UserRegistration userRegistration)
        {
            try
            {
                var result = await _userService.Register(userRegistration);
                if (result is null)
                    return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            AuthenticateResponse response = null;

            try
            {
                response = await _userService.Authenticate(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (response is null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(response.Token);
        }
    }
}
