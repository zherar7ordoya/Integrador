﻿using Integrador.Application.Interfaces.Infrastructure;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integrador.Infrastructure.Logging;

public class StructuredLogger(params IEnumerable<ILogWriter> writers) : ILogger
{
    public void LogInformation(string message) => Log(LogLevel.Information, message);
    public void LogError(string message) => Log(LogLevel.Error, message);
    public void LogError(Exception ex) => Log(LogLevel.Error, ex.Message, ex);
    public void LogError(Exception ex, string message) => Log(LogLevel.Error, message, ex);

    private void Log(LogLevel nivel, string mensaje, Exception? ex = null)
    {
        var entry = new LogEntry
        {
            Timestamp = DateTime.Now,
            Nivel = nivel,
            Mensaje = mensaje,
            StackTrace = ex?.ToString(),
            Fuente = GetCallerName()
        };

        foreach (var writer in writers)
            writer.Escribir(entry);
    }

    private static string? GetCallerName()
    {
        var method = new StackTrace().GetFrame(3)?.GetMethod();
        return $"{method?.DeclaringType?.Name}.{method?.Name}";
    }

    public void TryLog(string message, Exception? ex = null)
    {
        try { Log(ex == null ? LogLevel.Information : LogLevel.Error, message, ex); }
        catch (Exception loggingEx) { Debug.WriteLine($"Logging error: {loggingEx.Message}"); }
    }
}
