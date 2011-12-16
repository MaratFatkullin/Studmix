using System.IO;
using AI_.Studmix.Domain.Entities;

namespace AI_.Studmix.ApplicationServices.FileRepository
{
    public interface IFileRepository
    {
        Stream GetFileStream(ContentFile contentFile);
        void Store(ContentFile file, Stream stream);
    }
}