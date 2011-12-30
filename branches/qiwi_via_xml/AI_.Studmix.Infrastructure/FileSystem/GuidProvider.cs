using System;

namespace AI_.Studmix.Infrastructure.FileSystem
{
    public class GuidProvider : IGuidProvider
    {
        public Guid GetGuid()
        {
            return Guid.NewGuid();
        }
    }
}