using BlogStop.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace BlogStop.Services.Intefaces
{
    public interface IBlogPostService
    {

        #region BlogPost methods
        public Task AddBlogPostAsync(BlogPost blogPost);


        public Task UpdateBlogPostAsync(BlogPost blogPost);


        public Task<BlogPost> GetBlogPostAsync(int blogPostId);

        public Task<BlogPost> GetBlogPostAsync(string blogSlug);


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

        



        //public Task RemoveAllBlogPostAsync(int blogPostId);

        //public IEnumerable<BlogPost> Search(string serachString);

        public Task<bool> ValidateSlugAsync(string title, int blogId);






        public Task<IEnumerable<Tag>> GetTagsAsync();

        public Task AddtBlogPostToTagAsync(int tagId, int blogPostId);

        public Task AddBlogPostToTagsAsync(IEnumerable<int> tagIds, int blogPostId);

        public Task<IEnumerable<Tag>> GetAppUserTagsAsync(string appUserId);

        public Task<bool> IsBlogPostInTag(int tagId, int blogPostId);

        public Task RemoveAllBlogPostTagsAsync(int blogPostId);

        public Task AddTagsToBlogPostAsync(string stringTags, int blogPostId);

        public Task CreateCommentAsync(Comment comment);

        public IEnumerable<BlogPost> SearchBlogPosts(string? searchString);

        public IEnumerable<BlogPost> GetBlogPostsByCategory(int? categoryId);

        public Task<Tag> GetTagAsync(int tagId);

        public IEnumerable<Comment> GetAllComments();

        public Task<Comment> GetCommentAsync(int commentId);
        public Task UpdateCommentAsync(Comment comment);

        public Task DeleteCommentAsync(Comment comment);
        






    }
}
