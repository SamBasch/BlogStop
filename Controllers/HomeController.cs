using BlogStop.Data;
using BlogStop.Models;
using BlogStop.Services.Intefaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BlogStop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IBlogPostService _blogPostService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IBlogPostService blogPostService)
        {
            _logger = logger;
            _context = context;
            _blogPostService = blogPostService;

        }

        public async Task<IActionResult> Index()
        {

            //IEnumerable<BlogPost> blogPosts = await _context.BlogPosts.Include(b => b.Category).Where(b => b.IsPublished == true && b.IsDeleted == false).ToListAsync();

            IEnumerable<BlogPost> blogPosts = await _blogPostService.GetRecentBlogPosts();


            return View(blogPosts);
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