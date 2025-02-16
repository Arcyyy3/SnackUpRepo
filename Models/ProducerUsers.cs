using System;

namespace SnackUpAPI.Models
{
    public class ProducerUsers
    {
        public int ProducerID { get; set; }
        public int UserID { get; set; }

        public DateTime? Created { get; set; } = DateTime.UtcNow;
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
