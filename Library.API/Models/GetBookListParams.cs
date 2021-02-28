using Library.Application.DTOs;

namespace Library.API.Models
{
    public class GetBookListParams
    {
        public string Title { get; set; }
        public AuthorDTO Author { get; set; }
        public int? YearPublished { get; set; }
        public string Description { get; set; }
        public bool? InStock { get; set; }
    }
}