using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class SupportRequest
{
    public int SupportRequestId { get; set; }

    public int UserId { get; set; }

    public int? OrderId { get; set; }

    public string Subject { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual Order? Order { get; set; }

    public virtual User User { get; set; } = null!;
}
