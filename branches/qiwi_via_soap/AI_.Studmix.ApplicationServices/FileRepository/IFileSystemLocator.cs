using AI_.Studmix.Domain.Entities;

namespace AI_.Studmix.ApplicationServices.FileRepository
{
    public interface IFileSystemLocator
    {
        string GetLocation(ContentFile file);
    }
}