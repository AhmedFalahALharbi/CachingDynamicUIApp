using CachingDynamicUIApp.Models;
using CachingDynamicUIApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CachingDynamicUIApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly JsonPlaceholderService _jsonPlaceholderService;

        public PostsController(JsonPlaceholderService jsonPlaceholderService)
        {
            _jsonPlaceholderService = jsonPlaceholderService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetPosts()
        {
            var posts = await _jsonPlaceholderService.GetPostsAsync();
            return Ok(posts);
        }
    }
}