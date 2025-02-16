using System;

namespace SnackUpAPI.Models
{
    public class ClassDeliveryLog
    {
        public int LogID { get; set; }
        public int ClassDeliveryCodeID { get; set; }
        public int UserID { get; set; }
        public string CodeType { get; set; } = string.Empty; // "Code1" o "Code2"
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
