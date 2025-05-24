using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoruCevapPortal.Models;
using SoruCevapPortal.DTOs;

namespace SoruCevapPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public AdminController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }


        // kullanıcıları listeleme metodu
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _userManager.Users.Select(u => new UserDTO
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                RegistrationDate = u.RegistrationDate
            }).ToList();

            return Ok(users);
        }

        // kullanıcıları id ye göre listeleme metodu

        [HttpPost("users/{userId}/roles")]
        public async Task<IActionResult> AddUserToRole(string userId, [FromBody] string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found");

            if (!await _roleManager.RoleExistsAsync(roleName))
                return BadRequest("Role does not exist");

            await _userManager.AddToRoleAsync(user, roleName);
            return Ok();
        }

        // kullanıcı rolünü id ye göre listeleme

        [HttpDelete("users/{userId}/roles")]
        public async Task<IActionResult> RemoveUserFromRole(string userId, [FromBody] string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found");

            await _userManager.RemoveFromRoleAsync(user, roleName);
            return Ok();
        }

        // sistem istatiskiklerini listeleme

        [HttpGet("statistics")]
        public IActionResult GetStatistics()
        {
            var stats = new
            {
                TotalUsers = _userManager.Users.Count(),
                TotalQuestions = _context.Questions.Count(),
                TotalAnswers = _context.Answers.Count(),
                TotalComments = _context.Comments.Count(),
                TotalTags = _context.Tags.Count()
            };

            return Ok(stats);
        }
        // idye göre rol sorgulama
        [HttpGet("GetRoles/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(roles); // roller string listesi olarak döner
        }

    }
}