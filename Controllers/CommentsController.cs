using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogStop.Data;
using BlogStop.Models;
using Microsoft.AspNetCore.Identity;
using BlogStop.Services.Intefaces;
using BlogStop.Services;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace BlogStop.Controllers
{

    [Authorize(Roles = "Admin")]
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BlogUser> _userManager;
        private readonly IBlogPostService _blogPostService; 



        public CommentsController(ApplicationDbContext context, UserManager<BlogUser> userManager, IBlogPostService blogPostService)
        {
            _context = context;
            _userManager = userManager;
            _blogPostService = blogPostService;

       
            
        }

        // GET: Comments
        public IActionResult Index()
        {
            IEnumerable<Comment> comments =  _blogPostService.GetAllComments();

            return View(comments);
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            Comment comment = await _blogPostService.GetCommentAsync(id.Value);



            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        [AllowAnonymous]
        public IActionResult Create()
        {
           
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create([Bind("Id,Body,Created,Updated,UpdateReason,BlogPostId,AuthorId")] Comment comment, string? Slug)
        {

           


            ModelState.Remove("AuthorId");
            
            


            if (ModelState.IsValid)
            {
                
                comment.AuthorId = _userManager.GetUserId(User);

              comment.Created = DataUtility.GetPostGresDate(DateTime.UtcNow);


                await _blogPostService.CreateCommentAsync(comment);

                return RedirectToAction("Details", "BlogPosts", new { Slug });
            }
        
            return View(comment); 
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            Comment comment = await _blogPostService.GetCommentAsync(id.Value);

            if (comment == null)
            {
                return NotFound();
            }
            //ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", comment.AuthorId);
            //ViewData["BlogPostId"] = new SelectList(_context.BlogPosts, "Id", "Content", comment.BlogPostId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Body,Created,Updated,UpdateReason,BlogPostId,AuthorId")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                        await _blogPostService.UpdateCommentAsync(comment); 
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", comment.AuthorId);
            //ViewData["BlogPostId"] = new SelectList(_context.BlogPosts, "Id", "Content", comment.BlogPostId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            Comment comment = await _blogPostService.GetCommentAsync(id.Value);


            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Comment comment = await _blogPostService.GetCommentAsync(id.Value);
            if (comment != null)
            {
                 await _blogPostService.DeleteCommentAsync(comment);   
            }
            
          
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
          return (_context.Comments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
