using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Library.Application.DTOs
{
    public class BookLoanInfoDTO
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? BorrowedDate { get; set; }
        public DateTime? ReturnDueDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public BookLoanStatusDTO BookLoanStatus { get; set; }
    }
}