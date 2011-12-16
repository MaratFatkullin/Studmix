using System.Collections.Generic;

namespace AI_.Studmix.WebApplication.ViewModels.Content
{
    public class SearchViewModel
    {
        //public IEnumerable<Property> Properties { get; set; }

        public Dictionary<int, string> States { get; set; }

        //public IEnumerable<ContentPackage> Packages { get; set; }

        public SearchViewModel()
        {
            States = new Dictionary<int, string>();
        }
    }
}