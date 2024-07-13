using System;

namespace Domain.Shared
{
    /// <summary>
    /// Base class for entities.
    /// </summary>
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
    }
}