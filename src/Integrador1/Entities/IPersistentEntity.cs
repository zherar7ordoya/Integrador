﻿using System.Xml.Serialization;

namespace Integrador.Entities;

public interface IPersistentEntity
{
    int Id { get; set; }
    //DateTime CreatedAt { get; set; }
    //DateTime UpdatedAt { get; set; }
    //DateTime DeletedAt { get; set; }
    //bool IsDeleted { get; set; }
    //bool IsLocked { get; set; }
    //string CreatedBy { get; set; }
    //string UpdatedBy { get; set; }
    //string DeletedBy { get; set; }
}
