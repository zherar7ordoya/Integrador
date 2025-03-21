﻿using Integrador.Core;
using Integrador.CrossCutting;
using Integrador.Infrastructure.Repositories;

namespace Integrador.BusinessLogic.Commands.Personas;

public class DeletePersonaCommand(Persona persona) : ICommand
{
    public (bool Success, string ErrorMessage) Execute()
    {
        var (Success, Result, ErrorMessage) = SafeExecutor.Execute(() =>
        (
            new PersonaRepository().DeletePersona(persona)
        ));

        return (Success, ErrorMessage);
    }

    public void Undo() { /* Lógica para deshacer la eliminación (si es necesario) */ }
}
