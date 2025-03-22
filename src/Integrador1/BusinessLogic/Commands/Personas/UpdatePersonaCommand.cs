﻿using Integrador.Core;
using Integrador.CrossCutting;
using Integrador.Infrastructure.Repositories;

namespace Integrador.BusinessLogic.Commands.Personas;

public class UpdatePersonaCommand(Persona persona) : ICommand
{
    public (bool Success, string ErrorMessage) Execute()
    {
        var (Success, Result, ErrorMessage) = SafeExecutor.Execute(() =>
        (
            new PersonaRepository().UpdatePersona(persona)
        ));

        return (Success, ErrorMessage);
    }

    public void Undo() { /* Lógica para deshacer */ }
}
