using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogStop.Models
{
    public class Category
    {

        public int Id { get; set; }


        [Required]
        [Display(Name = "Category Name")]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at most {1} characters", MinimumLength = 2)]
        public string? Name { get; set; }   

        public string? Description { get; set; }    


        public byte[]? ImageData { get; set; }   

        public string? ImageType { get; set; }

        [NotMapped]
        public virtual IFormFile? Image { get; set; }


        //TODO: navigation properties


        public virtual ICollection<BlogPost> BlogPosts { get; set; } = new HashSet<BlogPost>();
    }
}
