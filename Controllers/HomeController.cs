using BlogStop.Data;
using BlogStop.Models;
using BlogStop.Services;
using BlogStop.Services.Intefaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly UserManager<BlogUser> _userManager;
        private readonly IEmailSender _emailService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IBlogPostService blogPostService, UserManager<BlogUser> userManager, IEmailSender emailSender)
        {
            _logger = logger;
            _context = context;
            _blogPostService = blogPostService;
            _userManager = userManager; 
            _emailService = emailSender;    


        }

        public async Task<IActionResult> Index(int? pageNum, string? swalMessage = null)
        {
            ViewData["SwalMessage"] = swalMessage;

            int pageSize = 3;
            int page = pageNum ?? 1;


           


            IPagedList<BlogPost> blogPosts = (await _blogPostService.GetRecentBlogPosts()).ToPagedList(page, pageSize);


            return View(blogPosts);
        }



        public async Task<IActionResult> AllPosts(int? pageNum)
        {


            int pageSize = 3;
            int page = pageNum ?? 1;





            IPagedList<BlogPost> blogPosts = (await _blogPostService.GetBlogPosts()).ToPagedList(page, pageSize);


            return View(blogPosts);
        }



        public async Task<IActionResult> PopularPosts(int? pageNum)
        {


            int pageSize = 3;
            int page = pageNum ?? 1;





            IPagedList<BlogPost> blogPosts = (await _blogPostService.GetPopularBlogPosts()).ToPagedList(page, pageSize);


            return View(blogPosts);
        }









        public async Task<IActionResult> ContactMe()
        {





            return View();
        }





        [HttpPost]

        public async Task<IActionResult> ContactMe(EmailData emailData)
        {




            if(ModelState.IsValid)
            {



                string? swalMessage = string.Empty;
                try
                {
                    emailData.EmailSubject = ($"{emailData.Name} Sent You A Message From BlogStop");
                    emailData.EmailBody = ($"""<strong>{emailData.Name}</strong> sent a message:<br><br>{emailData.EmailBody}<br><br><strong>Their email is:<a href="mailto:{emailData.EmailAddress}">{emailData.EmailAddress}</a></strong>""");
                    await _emailService.SendEmailAsync("baschnagelsam@gmail.com", emailData.EmailSubject, emailData.EmailBody!);
                    swalMessage = "Sucess! Your email has been sent.";
                    return RedirectToAction(nameof(Index), new { swalMessage });
                }
                catch (Exception)
                {
                    swalMessage = "Error! Your Email Failed to Send.";
                    return RedirectToAction(nameof(Index), new { swalMessage });
                    throw;
                }
            }

            


           








            return View(emailData);
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