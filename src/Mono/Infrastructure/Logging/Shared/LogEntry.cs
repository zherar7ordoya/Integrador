﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integrador.Infrastructure.Logging.Shared;

public class LogEntry
{
    public DateTime Timestamp { get; set; }
    public LogLevel Nivel { get; set; } = LogLevel.Information;
    public string Mensaje { get; set; } = string.Empty;
    public string? StackTrace { get; set; }
    public string? Fuente { get; set; }
}
