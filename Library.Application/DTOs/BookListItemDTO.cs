using System;

namespace Library.Application.DTOs
{
    public class BookListItemDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public bool InStock { get; set; }
        public DateTime? ReturnDueDate { get; set; }
    }
}