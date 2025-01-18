using System;
using System.Diagnostics;

public static class Utility
{
    public static string GetCurrentMoment()
    {
        var now = DateTime.Now.TimeOfDay;
        Debug.WriteLine($"ORARIO ATTUALE:{now.ToString()}");
        if (now >= new TimeSpan(10, 0, 0) && now <= new TimeSpan(10, 20, 0))
            return "First";
        if (now >= new TimeSpan(12, 0, 0) && now <= new TimeSpan(12, 20, 0))
            return "Second";

        return "OutsideHours";
    }
}
