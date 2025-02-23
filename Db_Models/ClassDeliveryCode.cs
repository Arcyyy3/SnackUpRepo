using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class ClassDeliveryCode
{
    public int ClassDeliveryCodeId { get; set; }

    public int SchoolClassId { get; set; }

    public string Code1 { get; set; } = null!;

    public string Code2 { get; set; } = null!;

    public bool? RetrievedCode1 { get; set; }

    public bool? RetrievedCode2 { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual ICollection<ClassDeliveryLog> ClassDeliveryLogs { get; set; } = new List<ClassDeliveryLog>();

    public virtual SchoolClass SchoolClass { get; set; } = null!;
}
