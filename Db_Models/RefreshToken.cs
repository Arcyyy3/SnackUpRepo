using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class RefreshToken
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpirationDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual User User { get; set; } = null!;
}
