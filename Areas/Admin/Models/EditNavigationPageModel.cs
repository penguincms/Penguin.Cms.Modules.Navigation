using Penguin.Cms.Navigation;

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
            BaseUri = baseUri;
            NavigationMenuItem = navigationMenuItem;
        }
    }
}