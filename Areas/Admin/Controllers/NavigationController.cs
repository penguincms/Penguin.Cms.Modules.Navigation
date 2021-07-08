using Microsoft.AspNetCore.Mvc;
using Penguin.Cms.Modules.Dynamic.Areas.Admin.Controllers;
using Penguin.Cms.Modules.Navigation.Areas.Admin.Models;
using Penguin.Cms.Modules.Navigation.Constants.Strings;
using Penguin.Cms.Navigation;
using Penguin.Cms.Navigation.Repositories;
using Penguin.Security.Abstractions;
using Penguin.Security.Abstractions.Interfaces;
using Penguin.Web.Security.Attributes;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Penguin.Cms.Modules.Navigation.Areas.Admin.Controllers
{
    [RequiresRole(RoleNames.CONTENT_MANAGER)]
    public class NavigationController : ObjectManagementController<NavigationMenuItem>
    {
        protected NavigationMenuRepository NavigationMenuRepository { get; set; }

        protected ISecurityProvider<NavigationMenuItem> SecurityProvider { get; set; }

        public NavigationController(NavigationMenuRepository navigationMenuRepository, ISecurityProvider<NavigationMenuItem> securityProvider, IServiceProvider serviceProvider, IUserSession userSession) : base(serviceProvider, userSession)
        {
            this.SecurityProvider = securityProvider;
            this.NavigationMenuRepository = navigationMenuRepository;
        }

        public ActionResult AddNavigation(string? Url = null)
        {
            EditNavigationPageModel model = new EditNavigationPageModel(Url, new NavigationMenuItem());

            return this.View(model);
        }

        [HttpPost]
        public ActionResult AddNavigation(EditNavigationPageModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.NavigationMenuItem is null)
            {
                throw new NullReferenceException("Posted NavigationMEnuItem is null");
            }

            using (this.NavigationMenuRepository.WriteContext())
            {
                if (!string.IsNullOrWhiteSpace(model.BaseUri))
                {
                    this.NavigationMenuRepository.AddChild(model.BaseUri, model.NavigationMenuItem);
                }
                else
                {
                    this.NavigationMenuRepository.AddOrUpdate(model.NavigationMenuItem);
                }
            }

            return this.Redirect("/Admin/Navigation/Index");
        }

        public ActionResult DeleteNavigation(int id)
        {
            using (this.NavigationMenuRepository.WriteContext())
            {
                NavigationMenuItem thisNavigation = this.NavigationMenuRepository.Find(id) ?? throw new NullReferenceException($"Can not find NavigationMenuItem with id {id}");

                thisNavigation.DateDeleted = DateTime.Now;
            }

            return this.Redirect("/Admin/Navigation/Index");
        }

        public ActionResult EditNavigation(string Url)
        {
            EditNavigationPageModel model = new EditNavigationPageModel(Url, this.NavigationMenuRepository.GetByUri(Url));

            if (model.NavigationMenuItem is null || !this.SecurityProvider.CheckAccess(model.NavigationMenuItem, PermissionTypes.Write))
            {
                return this.Content("");
            }

            return this.View(model);
        }

        [HttpPost]
        public ActionResult EditNavigation(EditNavigationPageModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.NavigationMenuItem is null)
            {
                throw new NullReferenceException(nameof(model.NavigationMenuItem));
            }

            using (this.NavigationMenuRepository.WriteContext())
            {
                NavigationMenuItem thisNavigation = this.NavigationMenuRepository.Find(model.NavigationMenuItem);

                if (this.SecurityProvider.CheckAccess(thisNavigation, PermissionTypes.Write))
                {
                    thisNavigation.Href = model.NavigationMenuItem.Href;
                    thisNavigation.Name = model.NavigationMenuItem.Name;
                    thisNavigation.Text = model.NavigationMenuItem.Text;
                    thisNavigation.Icon = model.NavigationMenuItem.Icon;

                    this.NavigationMenuRepository.AddOrUpdate(thisNavigation);
                }
            }

            return this.Redirect("/Admin/Navigation/Index");
        }
    }
}