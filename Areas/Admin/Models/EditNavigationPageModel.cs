using Penguin.Cms.Navigation;
using System.Diagnostics.CodeAnalysis;

namespace Penguin.Cms.Modules.Navigation.Areas.Admin.Models
{

    public class EditNavigationPageModel
    {
        public string? BaseUri { get; set; }

        public NavigationMenuItem? NavigationMenuItem { get; set; }

        public EditNavigationPageModel()
        {
        }

        public EditNavigationPageModel(string? baseUri, NavigationMenuItem navigationMenuItem)
        {
            this.BaseUri = baseUri;
            this.NavigationMenuItem = navigationMenuItem;
        }
    }
}