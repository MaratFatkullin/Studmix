using System;
using System.Collections.ObjectModel;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Tests;
using AI_.Studmix.Infrastructure.FileSystem;
using FluentAssertions;
using Xunit;

namespace AI_.Studmix.ApplicationServices.Tests.FileRepository
{
    public class FileSystemLocatorTestFixture : TestFixtureBase
    {
        public FileSystemLocator CreateSut()
        {
            return new FileSystemLocator();
        }
    }


    public class FileSystemLocatorTests : FileSystemLocatorTestFixture
    {
        [Fact]
        public void GetLocation_Simple_FolderNameCombinedFromPropertiIdAndStateIndex()
        {
            // Arrange
            var contentFile = CreateContentFile("file.txt", globalId: Guid.NewGuid());
            var contentPackage = CreateContentPackage(new Collection<ContentFile> { contentFile });
            var property1 = CreateProperty(id: 1);

            property1.GetState("state1");
            property1.GetState("state2");
            contentPackage.PropertyStates.Add(property1.GetState("state3"));

            var fileSystemLocator = CreateSut();

            // Act
            var location = fileSystemLocator.GetLocation(contentFile);

            // Assert
            location.Should().Be(string.Format(@"1_3\file-{0}.txt", contentFile.GlobalID));
        }

        [Fact]
        public void GetLocation_PropertiesUnordered_PathCombinedFromOrderedPropertyStates()
        {
            // Arrange
            var contentFile = CreateContentFile("file.txt", globalId: Guid.NewGuid());
            var contentPackage = CreateContentPackage(new Collection<ContentFile> { contentFile });
            var property1 = CreateProperty(id: 1, order: 2);
            var property2 = CreateProperty(id: 2, order: 1);
            contentPackage.PropertyStates.Add(property1.GetState("state"));
            contentPackage.PropertyStates.Add(property2.GetState("state"));

            var fileSystemLocator = CreateSut();

            // Act
            var location = fileSystemLocator.GetLocation(contentFile);

            // Assert
            location.Should().Be(string.Format(@"2_1\1_1\file-{0}.txt", contentFile.GlobalID));
        }

        [Fact]
        public void GetLocation_ThereIsUnspecifiedPropertyState_PathContainsDefaultFolderName()
        {
            // Arrange
            var contentFile = CreateContentFile("file.txt",globalId:Guid.NewGuid());
            var contentPackage = CreateContentPackage(new Collection<ContentFile> { contentFile });
            var property1 = CreateProperty(id: 1, order: 1);
            var property2 = CreateProperty(id: 2, order: 4);
            contentPackage.PropertyStates.Add(property1.GetState("state"));
            contentPackage.PropertyStates.Add(property2.GetState("state"));

            var fileSystemLocator = CreateSut();

            // Act
            var location = fileSystemLocator.GetLocation(contentFile);

            // Assert
            location.Should().Be(string.Format(@"1_1\-\-\2_1\file-{0}.txt", contentFile.GlobalID));
        }


        [Fact]
        public void GetLocation_PropertyStateListEmpty_PathContainsOnlyFileName()
        {
            // Arrange
            var contentFile = CreateContentFile("file.txt",globalId: Guid.NewGuid());
            var contentPackage = CreateContentPackage(new Collection<ContentFile> { contentFile });

            var fileSystemLocator = CreateSut();

            // Act
            var location = fileSystemLocator.GetLocation(contentFile);

            // Assert
            location.Should().Be(string.Format("file-{0}.txt", contentFile.GlobalID));
        }
    }
}