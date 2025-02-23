using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class SchoolClass
{
    public int SchoolClassId { get; set; }

    public int SchoolId { get; set; }

    public int ClassYear { get; set; }

    public string ClassSection { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual ICollection<ClassDeliveryCode> ClassDeliveryCodes { get; set; } = new List<ClassDeliveryCode>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual School School { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
