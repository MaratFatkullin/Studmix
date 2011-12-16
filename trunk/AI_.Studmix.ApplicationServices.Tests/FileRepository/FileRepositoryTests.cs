using System.IO;
using AI_.Studmix.ApplicationServices.FileRepository;
using AI_.Studmix.Domain.Tests;
using FluentAssertions;
using Moq;
using Xunit;

namespace AI_.Studmix.ApplicationServices.Tests.FileRepository
{
    public class FileRepositoryTestFixture : TestFixtureBase
    {
        protected Mock<IFileSystemLocator> FileSystemLocator;
        protected Mock<IFileSystemProvider> FileSystemProvider;

        public FileRepositoryTestFixture()
        {
            FileSystemProvider = new Mock<IFileSystemProvider>();
            FileSystemLocator = new Mock<IFileSystemLocator>();
        }

        public ApplicationServices.FileRepository.FileRepository CreateRepository()
        {
            return new ApplicationServices.FileRepository.FileRepository(FileSystemProvider.Object,
                                                                         FileSystemLocator.Object);
        }

        public Stream CreateStream()
        {
            return new Mock<Stream>().Object;
        }
    }


    public class FileRepositoryTests : FileRepositoryTestFixture
    {
        [Fact]
        public void Store_Simple_FileStoredToAppropriateLocation()
        {
            // Arrange
            var stream = CreateStream();
            var contentFile = CreateContentFile();

            FileSystemLocator.Setup(loc => loc.GetLocation(contentFile)).Returns("dir/filename");

            var fileRepository = CreateRepository();

            // Act
            fileRepository.Store(contentFile, stream);

            // Assert
            FileSystemProvider.Verify(provider => provider.Write("dir/filename", stream));
        }

        [Fact]
        public void GetFileStream_Simple_FileStreamReturned()
        {
            // Arrange
            Stream expectedStream = CreateStream();
            var contentFile = CreateContentFile();

            FileSystemLocator.Setup(l => l.GetLocation(contentFile)).Returns("location");
            FileSystemProvider.Setup(prov => prov.Read("location")).Returns(expectedStream);

            var repository = CreateRepository();

            // Act
            var stream = repository.GetFileStream(contentFile);

            // Assert
            stream.Should().Be(expectedStream);
        }
    }
}