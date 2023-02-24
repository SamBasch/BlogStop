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
using BlogStop.Helpers;

namespace BlogStop.Controllers
{

    [Authorize(Roles = "Admin")]
    public class BlogPostsController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly IBlogPostService _blogPostService;


        
        public BlogPostsController( IImageService imageService, IBlogPostService blogPostService)
        {
            
            _imageService = imageService;
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
        public async Task<IActionResult> Details(string? slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }


            Comment comment = new Comment();

            

            BlogPost blogPost = await _blogPostService.GetBlogPostAsync(slug);

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
        public async Task<IActionResult> Create([Bind("Id,Title,Abstract,Content,Created,Updated,Slug,IsDeleted,IsPublished,CategoryId,Image")] BlogPost blogPost, string? stringTags)
        {

            

            ModelState.Remove("Slug");


            if (ModelState.IsValid)
            {
               

                if (!await _blogPostService.ValidateSlugAsync(blogPost.Title!, blogPost.Id))
                {
                    ModelState.AddModelError("Title", "A similar Title or Slug is already in use.");

                    ViewData["CategoryList"] = new SelectList(await _blogPostService.GetCategoriesAsync(), "Id", "Name");

                    return View(blogPost);
                }

                blogPost.Slug = StringHelper.BlogSlug(blogPost.Title!);


                blogPost.Created = DataUtility.GetPostGresDate(DateTime.UtcNow);

                if (blogPost.Image != null)
                {
                    blogPost.ImageData = await _imageService.ConvertFileToByteArrayAsync(blogPost.Image);
                    blogPost.ImageType = blogPost.Image.ContentType;
                }

                

                await _blogPostService.AddBlogPostAsync(blogPost);

                if(!string.IsNullOrWhiteSpace(stringTags))
                {

                    await _blogPostService.AddTagsToBlogPostAsync(stringTags, blogPost.Id);
                }


                return RedirectToAction(nameof(AdminPage));
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

            IEnumerable<string> tagNames = blogPost.Tags.Select(t => t.Name!);

            ViewData["Tags"] = string.Join(", ", tagNames);


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
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,Abstract,Content,Created,Updated,Slug,IsDeleted,IsPublished,CategoryId,Image")] BlogPost blogPost, string? stringTags)
        {
            if (id != blogPost.Id)
            {
                return NotFound();
            }


            ModelState.Remove("Slug");

            if (ModelState.IsValid)
            {
                try
                {

                    if (!await _blogPostService.ValidateSlugAsync(blogPost.Title!, blogPost.Id))
                    {
                        ModelState.AddModelError("Title", "A similar Title or Slug is already in use.");

                        ViewData["CategoryList"] = new SelectList(await _blogPostService.GetCategoriesAsync(), "Id", "Name");

                        return View(blogPost);
                    }
                    blogPost.Slug = StringHelper.BlogSlug(blogPost.Title!);



                    blogPost.Created = DataUtility.GetPostGresDate(blogPost.Created);

                    blogPost.Updated = DataUtility.GetPostGresDate(DateTime.UtcNow);

                    if (blogPost.Image != null)
                    {
                        blogPost.ImageData = await _imageService.ConvertFileToByteArrayAsync(blogPost.Image);
                        blogPost.ImageType = blogPost.Image.ContentType;
                    }


               



                    await _blogPostService.UpdateBlogPostAsync(blogPost);   

                    await _blogPostService.RemoveAllBlogPostTagsAsync(blogPost.Id);

                    if (!string.IsNullOrWhiteSpace(stringTags))
                    {

                        await _blogPostService.AddTagsToBlogPostAsync(stringTags, blogPost.Id);
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
                return RedirectToAction(nameof(AdminPage));
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
            
            
            return RedirectToAction(nameof(AdminPage));
        }

        private async Task<bool> BlogPostExists(int? id)
        {
            return (await _blogPostService.GetBlogPosts()).Any(b => b.Id == id);
        }
    }
}
