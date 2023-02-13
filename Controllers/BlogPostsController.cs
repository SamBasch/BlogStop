using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogStop.Data;
using BlogStop.Models;
using Microsoft.AspNetCore.Authorization;
using BlogStop.Services;
using BlogStop.Services.Interfaces;
using System.Diagnostics;

namespace BlogStop.Controllers
{

    [Authorize(Roles = "Admin")]
    public class BlogPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly ITdListService _blogService;


        
        public BlogPostsController(ApplicationDbContext context, IImageService imageService, ITdListService blogService)
        {
            _context = context;
            _imageService = imageService;
            _blogService = blogService; 
        }

        // GET: BlogPosts
        [AllowAnonymous]
        public async Task<IActionResult> Index(int? categoryId, int? tagId)
        {


            IEnumerable<BlogPost> blogPosts = await _context.BlogPosts.Include(b => b.Comments).Include(b => b.Tags).Include(b => b.Category).ToListAsync();

            IEnumerable<Tag> tags = await _context.Tags.ToListAsync();

            // if (categoryId == null)
            //{

            //    tdItems = await _context.ToDoItems.Where(t => t.AppUserId == userId && t.Completed == false).Include(t => t.Accessories).ToListAsync();


            //}
            //else
            //{
            //    tdItems = (await _context.Accessories.Include(a => a.ToDoItems).FirstOrDefaultAsync(a => a.AppUserId == userId && a.Id == accessoryId))!.ToDoItems.ToList();
            //}



            //TODO: add int as 4th parameter
            //ViewData["CategoryList"] = new SelectList(CategoriesController,"Id", "Name",)


            ViewData["TagList"] = new SelectList(tags, "Id", "Name", tagId);
            return View(blogPosts);
        }

        // GET: BlogPosts/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BlogPosts == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Create
        
        public async Task<IActionResult> Create()
        {


            IEnumerable<Category> categoryList = await _context.Categories.ToListAsync();
            IEnumerable<Tag> tagList = await _context.Tags.ToListAsync();

            ViewData["CategoryList"] = new MultiSelectList(categoryList, "Id", "Name");

            ViewData["TagList"] = new MultiSelectList(tagList, "Id", "Name");

            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Abstract,Content,Created,Updated,Slug,IsDeleted,IsPublished,CategoryId,Image")] BlogPost blogPost, IEnumerable<int> selected)
        {
            if (ModelState.IsValid)
            {


                blogPost.Created = DataUtility.GetPostGresDate(DateTime.UtcNow);

                if (blogPost.Image != null)
                {
                    blogPost.ImageData = await _imageService.ConvertFileToByteArrayAsync(blogPost.Image);
                    blogPost.ImageType = blogPost.Image.ContentType;
                }



                _context.Add(blogPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", blogPost.CategoryId);
            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BlogPosts == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts.Include(b => b.Tags).FirstOrDefaultAsync(b => b.Id == id);


            IEnumerable<Category> categoryList = await _context.Categories.ToListAsync();

            IEnumerable<Tag> tagList = await _context.Tags.ToListAsync();

            IEnumerable<int> currentTags = blogPost!.Tags.Select(t => t.Id);
           

            ViewData["CategoryList"] = new SelectList(categoryList, "Id", "Name", blogPost.CategoryId);

            ViewData["TagList"] = new MultiSelectList(tagList, "Id", "Name", currentTags);


            if (blogPost == null)
            {
                return NotFound();
            }
            
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,Abstract,Content,Created,Updated,Slug,IsDeleted,IsPublished,CategoryId,Image")] BlogPost blogPost, IEnumerable<int> selected)
        {
            if (id != blogPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                   

                    blogPost.Created = DataUtility.GetPostGresDate(blogPost.Created);

                    blogPost.Updated = DataUtility.GetPostGresDate(DateTime.UtcNow);

                    if (blogPost.Image != null)
                    {
                        blogPost.ImageData = await _imageService.ConvertFileToByteArrayAsync(blogPost.Image);
                        blogPost.ImageType = blogPost.Image.ContentType;
                    }


                    _context.Update(blogPost);
                    await _context.SaveChangesAsync();

                    if (selected != null)
                    {
                        await _blogService.RemoveAllBlogPostTagsAsync(blogPost.Id);

                        await _blogService.AddBlogPostToTagsAsync(selected, blogPost.Id);

                        _context.Update(blogPost);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogPostExists(blogPost.Id))
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
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", blogPost.CategoryId);
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BlogPosts == null)
            {
                return NotFound();
            }


      
            var blogPost = await _context.BlogPosts
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
     
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.BlogPosts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BlogPosts'  is null.");
            }
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost != null)
            {
                _context.BlogPosts.Remove(blogPost);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogPostExists(int? id)
        {
          return (_context.BlogPosts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
