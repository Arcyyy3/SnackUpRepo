using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class Producer
{
    public int ProducerId { get; set; }

    public string ProducerName { get; set; } = null!;

    public string? Address { get; set; }

    public string? ContactInfo { get; set; }

    public string? PhotoLinkProduttore { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual ICollection<ProducerUser> ProducerUsers { get; set; } = new List<ProducerUser>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
