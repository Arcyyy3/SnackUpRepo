using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class InventoryHistory
{
    public int HistoryId { get; set; }

    public int ProductId { get; set; }

    public int ChangeAmount { get; set; }

    public string ChangeReason { get; set; } = null!;

    public string ChangedBy { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }

    public DateTime ChangeDate { get; set; }

    public virtual Product Product { get; set; } = null!;
}
