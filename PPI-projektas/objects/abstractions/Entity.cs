﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PPI_projektas.objects.abstractions
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { get; private set; }

        public Entity(bool createGUID = false) { 
            if(createGUID) {
                Id = Guid.NewGuid();
            }
        }
    }
}
