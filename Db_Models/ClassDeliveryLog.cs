using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class ClassDeliveryLog
{
    public int LogId { get; set; }

    public int ClassDeliveryCodeId { get; set; }

    public int UserId { get; set; }

    public string CodeType { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public virtual ClassDeliveryCode ClassDeliveryCode { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
