using System;
using System.ComponentModel.DataAnnotations;


namespace SnackUpAPI.Models
{
    public class School
    {
        public int SchoolID { get; set; }
        [Required]
        [MaxLength(255)]
        public string SchoolName { get; set; }
        [Required]
        [MaxLength(255)]
        public string Address { get; set; }
        public int? ProducerID { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
