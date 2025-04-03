﻿using Integrador.Application.Interfaces;

using ILogger = Integrador.Application.Interfaces.ILogger;

namespace Integrador.Application.Exceptions;

public class ExceptionHandler
(
    IMessenger messenger,
    ILogger logger
) : IExceptionHandler
{
    public void Handle(Exception ex)
    {
        logger.LogError(ex, $"Error capturado: {ex.Message}");
        messenger.ShowError(ex, $"{ex.Message}\nConsulte el log para más detalles.");
    }

    public void Handle(string msg)
    {
        logger.LogError($"Error capturado: {msg}");
        messenger.ShowError(new Exception(msg), $"{msg}\nConsulte el log para más detalles.");
    }

    public void Handle(Exception ex, string msg)
    {
        logger.LogError(ex, $"Error capturado: {msg}");
        messenger.ShowError(ex, $"{msg}\nConsulte el log para más detalles.");
    }
}
