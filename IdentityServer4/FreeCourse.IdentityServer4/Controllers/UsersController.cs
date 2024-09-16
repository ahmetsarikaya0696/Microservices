using FreeCourse.IdentityServer4.DTOs;
using FreeCourse.IdentityServer4.Models;
using FreeCourse.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace FreeCourse.IdentityServer4.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupDTO signupDTO)
        {
            var user = new ApplicationUser()
            {
                UserName = signupDTO.Username,
                Email = signupDTO.Email,
                City = signupDTO.City
            };

            var result = await _userManager.CreateAsync(user, signupDTO.Password);

            if (!result.Succeeded) return BadRequest(ResponseDTO<NoContentDTO>.Fail(result.Errors.Select(x => x.Description).ToList(), 400));

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);

            if (userClaim == null) return BadRequest();

            var user = await _userManager.FindByIdAsync(userClaim.Value);

            if (user == null) return BadRequest();

            return Ok(new { user.Id, user.UserName, user.Email, user.City });
        }

    }
}
