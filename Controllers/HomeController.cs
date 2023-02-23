using BlogStop.Data;
using BlogStop.Models;
using BlogStop.Services.Intefaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using X.PagedList;

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

        public async Task<IActionResult> Index(int? pageNum)
        {


            int pageSize = 3;
            int page = pageNum ?? 1;





            IPagedList<BlogPost> blogPosts = (await _blogPostService.GetRecentBlogPosts()).ToPagedList(page, pageSize);


            return View(blogPosts);
        }



        public async Task<IActionResult> ContactMe()
        {


          


            return View();
        }



        public IActionResult SearchIndex(string? searchString, int? pageNum) 
        {


            int pageSize = 5;
            int page = pageNum ?? 1;





            IPagedList<BlogPost> blogPosts = ( _blogPostService.SearchBlogPosts(searchString)).ToPagedList(page, pageSize);

            return View(nameof(Index), blogPosts);

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