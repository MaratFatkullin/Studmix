using System.Collections.Generic;
using AI_.Studmix.ApplicationServices.DataTransferObjects;
using MvcContrib.Pagination;

namespace AI_.Studmix.WebApplication.ViewModels.Content
{
    public class SearchViewModel
    {
        public IEnumerable<PropertyDto> Properties { get; set; }

        public Dictionary<int, string> States { get; set; }

        public IPagination<ContentPackageDto> PackagesPagination { get; set; }

        public int? PageNumber { get; set; }

        public SearchViewModel()
        {
            States = new Dictionary<int, string>();
        }
    }
}