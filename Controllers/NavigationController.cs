using Microsoft.AspNetCore.Mvc;
using Penguin.Cms.Navigation.Repositories;

namespace Penguin.Cms.Modules.Navigation.Controllers
{
    public class NavigationController : Controller
    {
        protected NavigationMenuRepository NavigationMenuRepository { get; set; }

        public NavigationController(NavigationMenuRepository navigationMenuRepository)
        {
            NavigationMenuRepository = navigationMenuRepository;
        }

        public ActionResult FindCurrentNav() => this.View(this.NavigationMenuRepository.GetByHref(this.Request.Path.Value));

        public ActionResult SelectNav(string MenuName) => this.View(this.NavigationMenuRepository.GetRootByName(MenuName));
    }
}