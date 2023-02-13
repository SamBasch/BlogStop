using BlogStop.Data;
using BlogStop.Models;
using BlogStop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogStop.Services
{
    public class BlogService : ITdListService
    {

        private readonly ApplicationDbContext _context;


        public BlogService(ApplicationDbContext context)
        {
            _context = context;
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

        public Task<IEnumerable<Tag>> GetAppUserTagsAsync(string appUserId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsBlogPostInTag(int tagId, int blogPostId)
        {
            BlogPost? blogPost = await _context.BlogPosts.Include(b => b.Tags).FirstOrDefaultAsync(b => b.Id == blogPostId);


            bool inTag = blogPost!.Tags.Select(t => t.Id).Contains(tagId);

            return inTag;
        }

        public async Task RemoveAllBlogPostTagsAsync(int blogPostId)
        {
            BlogPost? blogPost = await _context.BlogPosts.Include(b => b.Tags).FirstOrDefaultAsync(b => b.Id == blogPostId);

            blogPost!.Tags.Clear();
            _context.Update(blogPost);
            await _context.AddRangeAsync();
        }
    }
}
