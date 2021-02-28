namespace Library.Application.DTOs
{
    public class BookDetailsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public AuthorDTO Author { get; set; }
        public int YearPublished { get; set; }
        public string Description { get; set; }
        public bool InStock { get; set; }
        public BookLoanQueueDTO BookLoanQueue { get; set; }
    }
}