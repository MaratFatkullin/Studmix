using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace AI_.Studmix.WebApplication.ViewModels.Shared
{
    public class InformationViewModel
    {
        public string Title { get; set; }
        public ICollection<ActionLinkInfo> ActionLinks { get; set; }

        public InformationViewModel(string title)
        {
            Title = title;
            ActionLinks = new Collection<ActionLinkInfo>();
        }
    }


    public class ActionLinkInfo
    {
        public string LinkText { get; set; }
        public string Url { get; protected set; }

        public ActionLinkInfo(string url, string linkText)
        {
            LinkText = linkText;
            Url = url;
        }
    }
}