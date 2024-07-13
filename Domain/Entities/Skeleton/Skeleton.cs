using Domain.Shared;
using System;

namespace Domain.Entities.Skeleton
{

    public class Skeleton : Entity
    {
        public SkeletonName Name { get; private set; }
        public string Description { get; private set; }

        protected Skeleton()
        {
            // Required by EF!!!
        }

        public Skeleton(string name)
        {
            Name = new SkeletonName(name);
            Id = Guid.NewGuid();
            Description = name;
        }
    }
}
