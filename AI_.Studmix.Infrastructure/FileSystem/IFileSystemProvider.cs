using System.IO;

namespace AI_.Studmix.Infrastructure.FileSystem
{
    public interface IFileSystemProvider
    {
        void Write(string path, Stream inputStream);
        Stream Read(string path);
    }
}