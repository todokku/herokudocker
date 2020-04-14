using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using herokudocker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace herokudocker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("map")]
        [Authorize(AuthenticationSchemes = "Google")]
        public IActionResult Map()
        {
            return View("Map");
        }

        [HttpGet]
        [Route("getpost")]
        public async Task<IActionResult> GetPostById([FromQuery] int id)
        {
            ViewData["message"] = "Post wall";

            using (var context = new MyContext())
            {
                var post = await context.Posts.Where(x => x.Id == id).FirstOrDefaultAsync();
                var postsViewModels = new PostViewModel()
                {
                    Title = post.Title,
                    CreatedAt = post.CreatedAt,
                    ComentCount = post.CommentCount,
                    Content = post.Content,
                    Id = post.Id,
                    Url = post.Url
                };

                ViewData["post"] = postsViewModels;

                var comments = await context.Comments.Where(x => x.PostId == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();

                if (comments != null)
                {
                    var commentsViewModels = comments
                        .Select(x =>
                        new CommentViewModel
                        {
                            PostId = x.PostId,
                            Content = x.Content,
                            CreatedAt = x.CreatedAt
                        }).ToList();

                    ViewData["hascomments"] = true;
                    ViewData["comments"] = commentsViewModels;
                }
                else
                {
                    ViewData["hascomments"] = false;
                }
            }

            return View("ViewPostDetail");
        }

        [HttpPost]
        [HttpGet]
        [Route("signin-google")]
        public IActionResult SigninGoogle()
        {
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            ViewData["message"] = "Post wall";
            using (var context = new MyContext())
            {
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

                ViewData["posts"] = postsViewModels;
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

        [HttpPost]
        [Route("addcomment")]
        public async Task<IActionResult> AddComment([FromForm] CommentViewModel model)
        {
            if (string.IsNullOrEmpty(model?.Content))
            {
                return RedirectToAction("Error");
            }

            using (var context = new MyContext())
            {
                var comment = new Comment()
                {
                    CreatedAt = DateTime.UtcNow,
                    PostId = model.PostId,
                    Content = model.Content
                };

                context.Comments.Add(comment);

                var post = await context.Posts.Where(x => x.Id == model.PostId).SingleOrDefaultAsync();
                post.CommentCount += 1;

                await context.SaveChangesAsync();
            }

            return RedirectToAction("GetPostById", new { id = model.PostId });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
