using System.Collections.Generic;
using AI_.Studmix.Domain.Entities;

namespace AI_.Studmix.Infrastructure.FileSystem
{
    public interface IFileSystemLocator
    {
        string GetLocation(ContentFile file);
    }
}