





using X.PagedList;

namespace BlogStop.Models.ViewModels
{
    public class CategoryBlogPostViewModel
    {

        public Category? Category { get; set; }

        public IPagedList<BlogPost>? BlogPosts { get; set; }    


    }
}
