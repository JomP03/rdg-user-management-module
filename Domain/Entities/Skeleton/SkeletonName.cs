
using System;

namespace Domain.Entities.Skeleton
{

    public class SkeletonName
    {
        public string Value { get; private set; }

        protected SkeletonName()
        {
            // Required by EF!!!
        }

        public SkeletonName(string name)
        {
            Value = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
        }
    }
}
