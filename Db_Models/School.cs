using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class School
{
    public int SchoolId { get; set; }

    public string SchoolName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public int? ProducerId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual ICollection<SchoolClass> SchoolClasses { get; set; } = new List<SchoolClass>();
}
