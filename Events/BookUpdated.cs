using BookProject.Models;

namespace BookProject.Events
{
    public class BookUpdated : Event
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Author> Authors { get; set; }
        public DateTime? PublishedDate { get; set; }
        public override Guid Id => BookId;

        public string GetUpdatedValues()
        {
            var outputTemplate = "The {0} property was changed to: {1} ";
            var result = string.Empty;

            if (Title != null)
            {
                result += string.Format(outputTemplate, nameof(Title), Title);
            }

            if (Description != null)
            {
                result += string.Format(outputTemplate, nameof(Description), Description);
            }

            if (Authors != null)
            {
                result += string.Format(outputTemplate, nameof(Authors), string.Join(", ", Authors.Select(x => x.Name)));
            }

            if (PublishedDate != null)
            {
                result += string.Format(outputTemplate, nameof(PublishedDate), PublishedDate);
            }

            return result;
        }
    }
}
