using BookProject.Models;
using System.ComponentModel.DataAnnotations;

namespace BookProject.Contracts.Book.Requests
{
    public class CreateBookRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The Title field is required.")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The Description field is required.")]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The Authors field is required.")]
        public List<Author> Authors { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The PublishedDate field is required.")]
        public DateTime PublishedDate { get; set; }


    }
}
