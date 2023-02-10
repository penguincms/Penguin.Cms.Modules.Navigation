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

        public ActionResult FindCurrentNav()
        {
            return View(NavigationMenuRepository.GetByHref(Request.Path.Value));
        }

        public ActionResult SelectNav(string MenuName)
        {
            return View(NavigationMenuRepository.GetRootByName(MenuName));
        }
    }
}