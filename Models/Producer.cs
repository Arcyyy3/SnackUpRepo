using System;
using System.ComponentModel.DataAnnotations;


namespace SnackUpAPI.Models
{
    public class Producer
    {
        public int ProducerID { get; set; }
        [Required]
        [MaxLength(255)]
        public string ProducerName { get; set; }
        public string ContactInfo { get; set; }
        public string Address { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
