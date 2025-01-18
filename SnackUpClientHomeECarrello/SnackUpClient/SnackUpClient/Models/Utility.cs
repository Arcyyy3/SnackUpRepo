using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackUpClient.Models
{
    public static class Utility
    {
        public static string GetCurrentMoment()
        {
            var now = DateTime.UtcNow.AddHours(1).TimeOfDay;
            if (now >= new TimeSpan(10, 0, 0) && now <= new TimeSpan(10, 20, 0))
                return "First";
            if (now >= new TimeSpan(12, 0, 0) && now <= new TimeSpan(12, 20, 0))
                return "Second";

            return "OutsideHours";
        }
    }

}
