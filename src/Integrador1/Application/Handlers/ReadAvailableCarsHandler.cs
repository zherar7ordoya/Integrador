﻿using MediatR;
using Integrador.Domain.Entities;
using Integrador.Domain.Interfaces;
using Integrador.Application.Queries;

namespace Integrador.Application.Handlers;

public class ReadAvailableCarsHandler(
    IGenericRepository<Car> repository)
    : IRequestHandler<ReadAvailableCarsQuery, List<Car>>
{
    private readonly IGenericRepository<Car> _repository = repository;

    public Task<List<Car>> Handle(ReadAvailableCarsQuery request, CancellationToken ct)
    {
        // Obtiene autos sin dueño (Dueño == null en lugar de DueñoId == 0)
        var availableCars = _repository.GetAll()
            .Where(c => c.Dueño == null) // Usa la relación de dominio
            .ToList();

        return Task.FromResult(availableCars);
    }
}