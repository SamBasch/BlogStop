using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace BlogStop.Models
{
    public class EmailData


       
    {

        [Required]
        public string? EmailAddress { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? EmailBody { get; set; }

        [Required]
        public string? Name { get; set; }



    





    }
}
