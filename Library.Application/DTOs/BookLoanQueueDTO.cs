using System.Collections.Generic;

namespace Library.Application.DTOs
{
    public class BookLoanQueueDTO
    {
        public int QueueLength { get; set; }

        public List<UserDTO> Readers { get; set; }
    }
}