using CachingDynamicUIApp.Models;
using CachingDynamicUIApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CachingDynamicUIApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly JsonPlaceholderService _jsonPlaceholderService;

        public UsersController(JsonPlaceholderService jsonPlaceholderService)
        {
            _jsonPlaceholderService = jsonPlaceholderService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await _jsonPlaceholderService.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _jsonPlaceholderService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("{id}/posts")]
        public async Task<ActionResult<List<Post>>> GetUserPosts(int id)
        {
            var posts = await _jsonPlaceholderService.GetPostsByUserIdAsync(id);
            return Ok(posts);
        }
    }
}