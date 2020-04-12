using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using herokudocker.Models;
using Microsoft.EntityFrameworkCore;

namespace herokudocker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            using (var context = new MyContext())
            {
                ViewData["message"] = "Valami bolondság";

                var posts = await context.Posts.OrderByDescending(x => x.CreatedAt).ToListAsync();

                var postsViewModels = posts.Select(x => new PostViewModel()
                {
                    Title = x.Title,
                    CreatedAt = x.CreatedAt,
                    ComentCount = x.CommentCount,
                    Content = x.Content,
                    Id = x.Id,
                    Url = x.Url
                }).ToList();

                ViewData["videos"] = postsViewModels;
            }

            /*
            ViewData["posts"] = new List<PostViewModel>()
            { 
                new PostViewModel()
                {
                    Id = 123,
                    Title = "First post",
                    ComentCount = 0,
                    Content = "Yx sdfsdfs sdfsdf",
                    Url = "https://youtube.com",
                    CreatedAt = DateTime.UtcNow
                },
                new PostViewModel()
                {
                    Id = 12,
                    Title = "Second post",
                    ComentCount = 1,
                    Content = "content of second post",
                    Url = "https://dmmedia.com",
                    CreatedAt = DateTime.UtcNow
                },
            };
            */
            return View();
        }

        [HttpPost]
        [Route("addpost")]
        public async Task<IActionResult> AddPost([FromForm] PostViewModel model)
        {
            if (string.IsNullOrEmpty(model?.Title)) 
            {
                return RedirectToAction("Error");
            }

            using (var context = new MyContext())
            {
                var post = new Post()
                {
                    CommentCount = 0,
                    Title = model.Title,
                    Url = model.Url,
                    CreatedAt = DateTime.UtcNow,
                    Content = model.Content
                };
                context.Posts.Add(post);

                await context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
