using System;
using System.IO;
using AI_.Studmix.Domain.Entities;

namespace AI_.Studmix.ApplicationServices.FileRepository
{
    public class FileRepository : IFileRepository
    {
        protected IFileSystemProvider Provider { get; set; }
        protected IFileSystemLocator FileSystemLocator { get; set; }

        public FileRepository(IFileSystemProvider fileSystemProvider,
                              IFileSystemLocator fileSystemLocator)
        {
            Provider = fileSystemProvider;
            FileSystemLocator = fileSystemLocator;
        }

        #region IFileRepository Members

        public Stream GetFileStream(ContentFile contentFile)
        {
            var location = FileSystemLocator.GetLocation(contentFile);
            return Provider.Read(location);
        }

        public void Store(ContentFile file, Stream stream)
        {
            file.GlobalID = Guid.NewGuid();
            var location = FileSystemLocator.GetLocation(file);
            Provider.Write(location, stream);
        }

        #endregion
    }
}