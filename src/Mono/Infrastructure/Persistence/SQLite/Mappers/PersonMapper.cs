﻿using Integrador.Application.Interfaces;
using Integrador.Domain.Entities;
using Integrador.Infrastructure.Persistence.SQLite.Records;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integrador.Infrastructure.Persistence.SQLite.Mappers;

public class PersonMapper : IEntityMapper<Person, PersonRecord>
{
    public PersonRecord ToStorage(Person entity) => new()
    {
        Id = entity.Id,
        Nombre = entity.Nombre,
        Apellido = entity.Apellido,
        DNI = entity.DNI
    };

    public Person ToDomain(PersonRecord storage) => new()
    {
        Id = storage.Id,
        Nombre = storage.Nombre,
        Apellido = storage.Apellido,
        DNI = storage.DNI
    };
}
