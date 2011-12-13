using System.Collections.Generic;
using System.Linq;
using AI_.Studmix.ApplicationServices.Services.Abstractions;
using AI_.Studmix.ApplicationServices.Services.DataTransferObjects.SearchService.Requests;
using AI_.Studmix.ApplicationServices.Services.DataTransferObjects.SearchService.Responses;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Repository;

namespace AI_.Studmix.ApplicationServices.Services
{
    public class SearchService : ISearchService
    {
        protected IUnitOfWork UnitOfWork { get; set; }

        public SearchService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        #region ISearchService Members

        //public IEnumerable<PropertyState> GetBoundedStates(Property property, PropertyState state)
        //{
        //    var packages = state.ContentPackages;
        //    var propertyStates = packages.Aggregate(new List<PropertyState>().AsEnumerable(),
        //                                            (acc, elem) => acc.Concat(elem.PropertyStates));

        //    return propertyStates.Where(st => st.Property == property);
        //}

        public FindPackagesWithSamePropertyStatesResponse FindPackagesWithSamePropertyStates(
            FindPackagesWithSamePropertyStatesRequest request)

        {
            IEnumerable<ContentPackage> contentPackages = UnitOfWork.GetRepository<ContentPackage>().Get();

            foreach (var propertyStateInfo in request.PropertyStates)
            {
                if(string.IsNullOrEmpty(propertyStateInfo.Value))
                    continue;

                contentPackages = contentPackages
                    .Where(p => p.PropertyStates.Any(ps => ps.Property.ID == propertyStateInfo.Key &&
                                                           ps.Value == propertyStateInfo.Value))
                    .ToList();
            }
            var dictionary = contentPackages.ToDictionary(p => p.ID, p => p.Caption);
            return new FindPackagesWithSamePropertyStatesResponse {Packages = dictionary};
        }

        #endregion
    }
}