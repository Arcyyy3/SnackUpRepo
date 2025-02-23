using System;
using System.Collections.Generic;

namespace SnackUpAPI.Db_Models;

public partial class LoggerServer
{
    public int Id { get; set; }

    public DateTime Created { get; set; }

    public int LogType { get; set; }

    public string? LogMessage { get; set; }

    public string? StackTrace { get; set; }

    public string? LogSource { get; set; }
}
