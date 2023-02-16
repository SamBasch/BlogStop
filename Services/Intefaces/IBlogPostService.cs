using BlogStop.Models;
using System.Drawing;

namespace BlogStop.Services.Intefaces
{
    public interface IBlogPostService
    {

        #region BlogPost methods
        public Task AddBlogPostAsync(BlogPost blogPost);


        public Task UpdateBlogPostAsync(BlogPost blogPost);


        public Task<BlogPost> GetBlogPostAsync(int blogPostId);


        public Task DeleteBlogPostAsync(BlogPost blogPost);


        public Task<IEnumerable<BlogPost>> GetBlogPosts();

        public Task<IEnumerable<BlogPost>> GetPopularBlogPosts();

        public Task<IEnumerable<BlogPost>> GetPopularBlogPosts(int count);

        public Task<IEnumerable<BlogPost>> GetRecentBlogPosts();

        public Task<IEnumerable<BlogPost>> GetRecentBlogPosts(int count);

        #endregion




        #region Category CRUD
        public Task AddCategoryAsync(Category category);

        public Task UpdateCategoryAsync(Category category);


        public Task<IEnumerable<Category>> GetCategoriesAsync();
        public Task<Category> GetCategoryAsync(int categoryId);

        public Task DeleteCategoryAsync(Category category);


        #endregion

        public Task<IEnumerable<Tag>>GetTagsAsync();

    }
}
