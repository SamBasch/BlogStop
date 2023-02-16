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
using BlogStop.Services.Intefaces;

namespace BlogStop.Controllers
{

    [Authorize(Roles = "Admin")]
    public class BlogPostsController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly ITdListService _blogService;
        private readonly IBlogPostService _blogPostService;


        
        public BlogPostsController( IImageService imageService, ITdListService blogService, IBlogPostService blogPostService)
        {
            
            _imageService = imageService;
            _blogService = blogService;
            _blogPostService = blogPostService;
        }

        // GET: BlogPosts
        //[AllowAnonymous]
        //public async Task<IActionResult> Index(int? categoryId, int? tagId)
        //{


        //    //IEnumerable<BlogPost> blogPosts = await _context.BlogPosts.Include(b => b.Comments).Include(b => b.Tags).Include(b => b.Category).ToListAsync();

        //    //IEnumerable<Tag> tags = await _context.Tags.ToListAsync();



        //    //ViewData["TagList"] = new SelectList(tags, "Id", "Name", tagId);


        //   IEnumerable<BlogPost> blogPosts = await _blogPostService.GetBlogPosts();


        //    return View(blogPosts);
        //}

        // GET: BlogPosts/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BlogPost blogPost = await _blogPostService.GetBlogPostAsync(id.Value);

            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }






        public async Task<IActionResult> AdminPage()
        {

            IEnumerable<BlogPost> blogPosts = await _blogPostService.GetBlogPosts();
            return View(blogPosts);
        }






        // GET: BlogPosts/Create
        
        public async Task<IActionResult> Create()
        {




            IEnumerable<Tag> tags = await _blogPostService.GetTagsAsync();

            ViewData["CategoryList"] = new SelectList(await _blogPostService.GetCategoriesAsync(), "Id", "Name") ;

            ViewData["TagList"] = new MultiSelectList(tags, "Id", "Name");

            return View(new BlogPost());
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

                await _blogPostService.AddBlogPostAsync(blogPost);  

                
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", blogPost.CategoryId);
            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BlogPost blogPost = await _blogPostService.GetBlogPostAsync(id.Value);

            IEnumerable<int> currentTags = blogPost!.Tags.Select(t => t.Id);
           

            ViewData["CategoryList"] = new SelectList(await _blogPostService.GetCategoriesAsync(), "Id", "Name", blogPost.CategoryId);

            ViewData["TagList"] = new MultiSelectList(await _blogPostService.GetTagsAsync(), "Id", "Name", currentTags);


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


                        await _blogPostService.UpdateBlogPostAsync(blogPost);   

                    if (selected != null)
                    {
                        await _blogService.RemoveAllBlogPostTagsAsync(blogPost.Id);

                        await _blogService.AddBlogPostToTagsAsync(selected, blogPost.Id);

                        await _blogPostService.UpdateBlogPostAsync(blogPost);
                    
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await BlogPostExists(blogPost.Id))
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
            if (id == null)
            {
                return NotFound();
            }



            BlogPost blogPost = await _blogPostService.GetBlogPostAsync(id.Value);


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
            if (id == null)
            {
                return NotFound();
            }



            BlogPost blogPost = await _blogPostService.GetBlogPostAsync(id.Value);

            if (blogPost != null)
            {
                await _blogPostService.DeleteBlogPostAsync(blogPost);
            }
            
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> BlogPostExists(int? id)
        {
            return (await _blogPostService.GetBlogPosts()).Any(b => b.Id == id);
        }
    }
}
