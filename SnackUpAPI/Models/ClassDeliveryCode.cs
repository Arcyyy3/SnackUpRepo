using System;

namespace SnackUpAPI.Models
{
    public class ClassDeliveryCode
    {
        public int ClassDeliveryCodeID { get; set; }
        public int SchoolClassID { get; set; }
        public string Code1 { get; set; } = string.Empty;
        public string Code2 { get; set; } = string.Empty;
        public bool RetrievedCode1 { get; set; } = false;
        public bool RetrievedCode2 { get; set; } = false;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
