using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserID { get; set; }

    [Required]
    [MaxLength(100)]
    public string UserName { get; set; }

    [Required]
    [MaxLength(100)]
    public string Surname { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [MaxLength(255)]
    public string Email { get; set; }

    [Required]
    [MaxLength(50)]
    public string Role { get; set; }

    [Required]
    public DateTime RegistrationDate { get; set; }

    public int? SchoolClassID { get; set; }

    public DateTime? Created { get; set; } = DateTime.Now;

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }
}
