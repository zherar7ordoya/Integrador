﻿using Integrador.Core;
using Integrador.CrossCutting;

namespace Integrador.BusinessLogic.Commands;

public class EliminarAutoCommand(ViewController viewController, Auto auto) : ICommand
{
    private readonly ViewController _viewController = viewController;
    private readonly Auto _auto = auto;

    public (bool Success, string ErrorMessage) Execute()
    {
        var (Success, Result, ErrorMessage) = SafeExecutor.Execute(() => _viewController.EliminarAuto(_auto));
        return (Success, ErrorMessage);
    }

    public void Undo() { /* Lógica para deshacer */ }
}
