using System.Collections.ObjectModel;
using System.Linq;
using AI_.Studmix.ApplicationServices.DataTransferObjects;
using AI_.Studmix.ApplicationServices.DataTransferObjects.Mapper;
using AI_.Studmix.ApplicationServices.FileRepository;
using AI_.Studmix.ApplicationServices.Services.ContentService.Requests;
using AI_.Studmix.ApplicationServices.Services.ContentService.Responses;
using AI_.Studmix.ApplicationServices.Specifications;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Factories;
using AI_.Studmix.Domain.Repository;
using AI_.Studmix.Domain.Services.Abstractions;

namespace AI_.Studmix.ApplicationServices.Services.ContentService
{
    public class ContentService : IContentService
    {
        protected IUnitOfWork UnitOfWork { get; set; }
        protected IFileRepository FileRepository { get; set; }
        protected IPermissionService PermissionService { get; set; }

        public ContentService(IUnitOfWork unitOfWork,
                              IFileRepository fileRepository,
                              IPermissionService permissionService)
        {
            UnitOfWork = unitOfWork;
            FileRepository = fileRepository;
            PermissionService = permissionService;
        }

        #region IContentService Members

        public GetPropertiesResponse GetProperties()
        {
            var properties = UnitOfWork.GetRepository<Property>().Get();

            var propertyDtos = DtoMapper.MapSequence<PropertyDto>(properties.OrderBy(pr => pr.Order));
            return new GetPropertiesResponse(propertyDtos);
        }

        public void Store(StoreRequest request)
        {
            var userRepository = UnitOfWork.GetRepository<User>();
            User owner = userRepository.Get(new GetUserByUserName(request.OwnerUserName)).Single();

            //property states
            var propertyRepository = UnitOfWork.GetRepository<Property>();
            var propertyStates = new Collection<PropertyState>();
            foreach (var state in request.States)
            {
                var property = propertyRepository.GetByID(state.Key);
                var propertyState = property.GetState(state.Value);

                propertyStates.Add(propertyState);
            }

            var factory = new ContentPackageFactory();
            var contentPackage = factory.CreateContentPackage(request.Caption,
                                                              request.Description,
                                                              owner,
                                                              request.Price,
                                                              propertyStates);

            //files
            foreach (var fileInfo in request.PreviewContentFiles)
            {
                var contentFile = contentPackage.AddFile(fileInfo.FileName, true);
                FileRepository.Store(contentFile, fileInfo.Stream);
            }
            foreach (var fileInfo in request.ContentFiles)
            {
                var contentFile = contentPackage.AddFile(fileInfo.FileName, false);
                FileRepository.Store(contentFile, fileInfo.Stream);
            }


            var repository = UnitOfWork.GetRepository<ContentPackage>();
            repository.Insert(contentPackage);

            UnitOfWork.Save();
        }

        public GetPackageByIDResponse GetPackageByID(GetPackageByIDRequest request)
        {
            var repository = UnitOfWork.GetRepository<ContentPackage>();
            var contentPackage = repository.GetByID(request.ID);

            if (contentPackage == null)
                return new GetPackageByIDResponse(null, false);

            var user = UnitOfWork.GetRepository<User>().Get(new GetUserByUserName(request.UserName)).Single();
            var userHasPermissions = PermissionService.UserHasPermissions(user, contentPackage);

            var contentPackageDto = DtoMapper.Map<ContentPackageDto>(contentPackage);

            return new GetPackageByIDResponse(contentPackageDto, userHasPermissions);
        }

        public DownloadResponse DownloadFile(DownloadRequest request)
        {
            var contentFile = UnitOfWork.GetRepository<ContentFile>().GetByID(request.FileID);
            var user = UnitOfWork.GetRepository<User>().Get(new GetUserByUserName(request.UserName)).Single();
            var userHasPermissions = PermissionService.UserHasPermissions(user, contentFile.ContentPackage);
            userHasPermissions |= contentFile.IsPreview;
            if (userHasPermissions)
            {
                var stream = FileRepository.GetFileStream(contentFile);
                return new DownloadResponse(new FileStreamDto(contentFile.Name, stream, contentFile.IsImage));
            }
            else
            {
                return new DownloadResponse();
            }
        }

        #endregion
    }
}