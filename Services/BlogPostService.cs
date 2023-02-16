using BlogStop.Controllers;
using BlogStop.Data;
using BlogStop.Models;
using BlogStop.Services.Intefaces;
using BlogStop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogStop.Services
{
    public class BlogPostService : IBlogPostService
    {

        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public BlogPostService(ApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }



        public async Task AddBlogPostAsync(BlogPost blogPost)
        {

            try
            {

                await _context.AddAsync(blogPost);
                await _context.SaveChangesAsync(); 



            }
            catch (Exception)
            {

                throw;
            }


          
        }

        public async Task AddCategoryAsync(Category category)
        {
            try
            {

                await _context.AddAsync(category);
                await _context.SaveChangesAsync();


            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteBlogPostAsync(BlogPost blogPost)
        {

            try
            {


                blogPost.IsDeleted = true;
                await UpdateBlogPostAsync(blogPost);



            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task DeleteCategoryAsync(Category category)
        {

            try
            {

                 _context.Remove(category);
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }




        }

        public async Task<BlogPost> GetBlogPostAsync(int blogPostId)
        {


            try
            {

                BlogPost? blogPost = await _context.BlogPosts.Include(b => b.Category).Include(b => b.Tags).Include(b => b.Comments).FirstOrDefaultAsync(blogPost => blogPost.Id == blogPostId);

                return blogPost!;
            }
            catch (Exception)
            {

                throw;
            }


            

            

            
        }

        public async Task<IEnumerable<BlogPost>> GetBlogPosts()
        {
            try
            {


              IEnumerable<BlogPost> blogPosts = await _context.BlogPosts.Include(b => b.Tags).Include(b => b.Comments).Include(b => b.Category).ToListAsync();

                return blogPosts;

            }
            catch (Exception)
            {

                throw;
            }
        }



        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {


            try
            {
                IEnumerable<Category> categories = await _context.Categories.ToListAsync();
                return categories;

            }
            catch (Exception)
            {

                throw;
            }
          

        }

        public async Task<Category> GetCategoryAsync(int categoryId)
        {


            try
            {

              Category? category = await  _context.Categories.Include(c => c.BlogPosts).FirstOrDefaultAsync(c => c.Id == categoryId);
                return category!;


              

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<IEnumerable<BlogPost>> GetPopularBlogPosts()
        {
            try
            {


                IEnumerable<BlogPost> blogPosts = await _context.BlogPosts.Where(b => b.IsPublished == true && b.IsDeleted == false).Include(b => b.Category).Include(b => b.Tags).Include(b => b.Comments).ThenInclude(c => c.Author).ToListAsync();


                return blogPosts.OrderByDescending(b => b.Comments.Count);


            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BlogPost>> GetPopularBlogPosts(int count)
        {
            try
            {


                IEnumerable<BlogPost> blogPosts = await _context.BlogPosts.Where(b => b.IsPublished == true && b.IsDeleted == false).Include(b => b.Category).Include(b => b.Tags).Include(b => b.Comments).ThenInclude(c => c.Author).ToListAsync();


                return blogPosts.OrderByDescending(b => b.Comments.Count);


            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<BlogPost>> GetRecentBlogPosts()
        {
            try
            {


                IEnumerable<BlogPost> blogPosts = await _context.BlogPosts.Where(b => b.IsPublished == true && b.IsDeleted == false).Include(b => b.Category).Include(b => b.Tags).Include(b => b.Comments).ThenInclude(c => c.Author).ToListAsync();


                return blogPosts.OrderByDescending(b => b.Created);


            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<BlogPost>> GetRecentBlogPosts(int count)
        {
            try
            {


                IEnumerable<BlogPost> blogPosts = await _context.BlogPosts.Where(b => b.IsPublished == true && b.IsDeleted == false).Include(b => b.Category).Include(b => b.Tags).Include(b => b.Comments).ThenInclude(c => c.Author).ToListAsync();


                return blogPosts.OrderByDescending(b => b.Created).Take(count);


            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public async Task UpdateBlogPostAsync(BlogPost blogPost)
        {
            try
            {

                _context.Update(blogPost);
                await _context.SaveChangesAsync();  

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateCategoryAsync(Category category)
        {

            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

            
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync()
        {
            try
            {
                IEnumerable<Tag> tags = await _context.Tags.ToListAsync();
                return tags;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
