using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class ProducerUser
{
    public int ProducerId { get; set; }

    public int UserId { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual Producer Producer { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
