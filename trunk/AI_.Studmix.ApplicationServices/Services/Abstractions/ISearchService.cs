using System.Collections.Generic;
using AI_.Studmix.ApplicationServices.Services.DataTransferObjects.SearchService.Requests;
using AI_.Studmix.ApplicationServices.Services.DataTransferObjects.SearchService.Responses;
using AI_.Studmix.Domain.Entities;

namespace AI_.Studmix.ApplicationServices.Services.Abstractions
{
    public interface ISearchService
    {
        FindPackagesWithSamePropertyStatesResponse FindPackagesWithSamePropertyStates(
            FindPackagesWithSamePropertyStatesRequest request);
    }
}