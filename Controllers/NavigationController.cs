using Microsoft.AspNetCore.Mvc;
using Penguin.Cms.Navigation.Repositories;

namespace Penguin.Cms.Modules.Navigation.Controllers
{
    public class NavigationController : Controller
    {
        protected NavigationMenuRepository NavigationMenuRepository { get; set; }

        public NavigationController(NavigationMenuRepository navigationMenuRepository)
        {
            this.NavigationMenuRepository = navigationMenuRepository;
        }

        public ActionResult FindCurrentNav()
        {
            return this.View(this.NavigationMenuRepository.GetByHref(this.Request.Path.Value));
        }

        public ActionResult SelectNav(string MenuName)
        {
            return this.View(this.NavigationMenuRepository.GetRootByName(MenuName));
        }
    }
}