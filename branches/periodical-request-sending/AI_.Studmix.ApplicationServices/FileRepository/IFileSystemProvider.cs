using System.IO;

namespace AI_.Studmix.ApplicationServices.FileRepository
{
    public interface IFileSystemProvider
    {
        void Write(string path, Stream inputStream);
        Stream Read(string path);
    }
}