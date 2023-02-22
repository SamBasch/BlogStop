using BlogStop.Controllers;
using BlogStop.Data;
using BlogStop.Helpers;
using BlogStop.Models;
using BlogStop.Services.Intefaces;
using BlogStop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
                IEnumerable<Tag> tags = await _context.Tags.Include(t => t.BlogPosts).ToListAsync();
                return tags;
            }
            catch (Exception)
            {

                throw;
            }
        }




        public async Task<bool> ValidateSlugAsync(string title, int blogId)
        {


            try
            {
                string newSlug = StringHelper.BlogSlug(title);

                if(blogId == 0)
                {
                    return !await _context.BlogPosts.AnyAsync(b => b.Slug == newSlug);
                } else
                {
                    BlogPost? blogPost = await _context.BlogPosts.AsNoTracking().FirstOrDefaultAsync(b => b.Id == blogId);

                }

                return true;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<BlogPost> GetBlogPostAsync(string blogSlug)
        {
            try
            {

                BlogPost? blogPost = await _context.BlogPosts.Include(b => b.Category).Include(b => b.Tags).Include(b => b.Comments).ThenInclude(c => c.Author).FirstOrDefaultAsync(b => b.Slug == blogSlug);

                return blogPost!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task AddtBlogPostToTagAsync(int tagId, int blogPostId)
        {
            throw new NotImplementedException();
        }

        public async Task AddBlogPostToTagsAsync(IEnumerable<int> tagIds, int blogPostId)
        {
            try
            {

                BlogPost? blogPost = await _context.BlogPosts.Include(b => b.Tags).FirstOrDefaultAsync(b => b.Id == blogPostId);



                foreach (int tagId in tagIds)
                {
                    Tag? tag = await _context.Tags.FindAsync(tagId);

                    if (blogPost != null && tag != null)
                    {
                        blogPost.Tags.Add(tag);
                    }
                }

                await _context.SaveChangesAsync();


            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task AddTagsToBlogPostAsync(string stringTags, int blogPostId)
        {

            try
            {
                BlogPost? blogPost = await _context.BlogPosts.FindAsync(blogPostId);

                if (blogPost == null)
                {
                    return;
                }


                foreach (string tagName in stringTags.Split(","))
                {



                    if (string.IsNullOrWhiteSpace(tagName.Trim()))
                    {
                        continue;   
                    }

                    Tag? tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.Trim().ToLower() == tagName.Trim().ToLower());

                    if (tag != null)
                    {
                        blogPost.Tags.Add(tag);
                    }
                    else
                    {



                        Tag newTag = new Tag() { Name = tagName.Trim() };

                        blogPost.Tags.Add(newTag);
                    }
                }

                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<IEnumerable<Tag>> GetAppUserTagsAsync(string appUserId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsBlogPostInTag(int tagId, int blogPostId)
        {

            try
            {
                BlogPost? blogPost = await _context.BlogPosts.Include(b => b.Tags).FirstOrDefaultAsync(b => b.Id == blogPostId);


                bool inTag = blogPost!.Tags.Select(t => t.Id).Contains(tagId);

                return inTag;
            }
            catch (Exception)
            {

                throw;
            }


            
        }

        public async Task RemoveAllBlogPostTagsAsync(int blogPostId)
        {


            try
            {
                BlogPost? blogPost = await _context.BlogPosts.Include(b => b.Tags).FirstOrDefaultAsync(b => b.Id == blogPostId);

                if(blogPost != null)
                {

                    blogPost.Tags.Clear();
                    _context.Update(blogPost);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
     
        }

        public async Task CreateCommentAsync(Comment comment)
        {

            try
            {


                

                await _context.AddAsync(comment);
                await _context.SaveChangesAsync();


            }
            catch (Exception)
            {

                throw;
            }

        }


        public Task AddCommentToBlogPost(int tagId, int blogPostId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BlogPost> SearchBlogPosts(string? searchString)
        {
            try
            {
                IEnumerable<BlogPost> blogPosts = new List<BlogPost>();

                if(string.IsNullOrEmpty(searchString))
                {
                    return blogPosts;
                } else
                {
                    searchString = searchString.Trim().ToLower();

                    blogPosts = _context.BlogPosts.Where(b => b.Title!.ToLower().Contains(searchString) ||
                                                        b.Abstract!.ToLower().Contains(searchString) ||
                                                        b.Content!.ToLower().Contains(searchString) ||
                                                        b.Category!.Name!.ToLower().Contains(searchString) ||
                                                        b.Comments.Any(c => c.Body!.ToLower().Contains(searchString) ||
                                                                       c.Author!.FirstName!.ToLower().Contains(searchString) ||
                                                                       c.Author!.LastName!.ToLower().Contains(searchString)) ||
                                                        b.Tags.Any(t => t.Name!.ToLower().Contains(searchString)))
                                                   .Include(b => b.Comments)
                                                           .ThenInclude(c => c.Author)
                                                   .Include(b => b.Category)
                                                   .Include(b => b.Tags)
                                                   .Where(b => b.IsPublished == true && b.IsDeleted == false)
                                                   .AsNoTracking()
                                                   .OrderByDescending(b => b.Created)
                                                   .AsEnumerable();

                    return blogPosts;

                                                                       
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public  IEnumerable<BlogPost> GetBlogPostsByCategory(int? categoryId)
        {
            try
            {
                IEnumerable<BlogPost> blogPosts = _context.BlogPosts.Include(b => b.Tags).Include(b => b.Comments).Include(b => b.Category).Where(b => b.CategoryId == categoryId);

                return blogPosts;
            }
            catch (Exception)
            {

                throw;
            }

           


        }

        public async Task<Tag> GetTagAsync(int tagId)
        {

            try
            {

                Tag? tag = await _context.Tags.Include(c => c.BlogPosts).FirstOrDefaultAsync(c => c.Id == tagId);
                return tag!;




            }
            catch (Exception)
            {

                throw;
            }
        }


        public IEnumerable<Comment> GetAllComments()
        {

            try
            {
                IEnumerable<Comment> comments =  _context.Comments.Include(c => c.Author).Include(c => c.BlogPost);

                return comments;
            }
            catch (Exception)
            {

                throw;
            }
            




        }

        public async Task<Comment> GetCommentAsync(int commentId)
        {


            try
            {
                Comment? comment = await _context.Comments.Include(c => c.BlogPost).Include(c => c.Author).FirstOrDefaultAsync(c => c.Id == commentId);

                return comment!;
            }
            catch (Exception)
            {

                throw;
            }

            
        

        }


        public async Task UpdateCommentAsync(Comment comment)
        {
            try
            {
                _context.Update(comment);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
            

        }

        public async Task DeleteCommentAsync(Comment comment)
        {
            try
            {

                _context.Remove(comment);
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }


      
    }
}
