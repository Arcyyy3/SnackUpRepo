using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime RegistrationDate { get; set; }

    public int? SchoolClassId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual ICollection<ClassDeliveryLog> ClassDeliveryLogs { get; set; } = new List<ClassDeliveryLog>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ProducerUser> ProducerUsers { get; set; } = new List<ProducerUser>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual SchoolClass? SchoolClass { get; set; }

    public virtual ICollection<ShoppingSession> ShoppingSessions { get; set; } = new List<ShoppingSession>();

    public virtual ICollection<SupportRequest> SupportRequests { get; set; } = new List<SupportRequest>();

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}
