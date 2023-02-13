using BlogStop.Models;

namespace BlogStop.Services.Interfaces
{
    public interface ITdListService
    {
        public Task AddtBlogPostToTagAsync(int tagId, int blogPostId);

        public Task AddBlogPostToTagsAsync(IEnumerable<int> tagIds, int blogPostId);

        public Task<IEnumerable<Tag>> GetAppUserTagsAsync(string appUserId);

        public Task<bool> IsBlogPostInTag(int tagId, int blogPostId);

        public Task RemoveAllBlogPostTagsAsync(int blogPostId);
    }
}
