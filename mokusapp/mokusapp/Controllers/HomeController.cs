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
                var videos = await context.Videos
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(1000)
                    .ToListAsync();

                var videoViewModels = videos.Select(x => new VideoViewModel()
                {
                    EmbedCode = x.EmbedCode,
                    Desc = x.Desc,
                    CreatedAt = x.CreatedAt
                }).ToList();

                ViewData["message"] = "What youtube video are you wathcing?";
                ViewData["videos"] = videoViewModels;
            }

            return View();
        }

        [HttpPost]
        [Route("addvideo")]
        public async Task<IActionResult> AddVideoAsync([FromForm] VideoViewModel model)
        {
            using (var context = new MyContext())
            {
                context.Videos.Add(new Video()
                {
                    EmbedCode = model.EmbedCode,
                    CreatedAt = DateTime.UtcNow, 
                    Desc = model.Desc
                });

                await context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
