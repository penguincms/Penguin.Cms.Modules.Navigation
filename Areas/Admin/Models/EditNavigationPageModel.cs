using Penguin.Cms.Navigation;
using System.Diagnostics.CodeAnalysis;

namespace Penguin.Cms.Modules.Navigation.Areas.Admin.Models
{
    [SuppressMessage("Design", "CA1056:Uri properties should not be strings")]
    [SuppressMessage("Design", "CA1054:Uri parameters should not be strings")]
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