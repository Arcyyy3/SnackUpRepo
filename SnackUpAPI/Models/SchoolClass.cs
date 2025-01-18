using System;
using System.ComponentModel.DataAnnotations;


namespace SnackUpAPI.Models
{
    public class SchoolClass
    {
        public int SchoolClassID { get; set; }
        public int SchoolID { get; set; }
        public int ClassYear { get; set; }
        [Required]
        [MaxLength(10)]
        public string ClassSection { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
