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

namespace Penguin.Cms.Modules.Navigation.Areas.Admin.Controllers
{
	[RequiresRole(RoleNames.CONTENT_MANAGER)]
	public class NavigationController : ObjectManagementController<NavigationMenuItem>
	{
		protected NavigationMenuRepository NavigationMenuRepository { get; set; }

		protected ISecurityProvider<NavigationMenuItem> SecurityProvider { get; set; }

		public NavigationController(NavigationMenuRepository navigationMenuRepository, ISecurityProvider<NavigationMenuItem> securityProvider, IServiceProvider serviceProvider, IUserSession userSession) : base(serviceProvider, userSession)
		{
			SecurityProvider = securityProvider;
			NavigationMenuRepository = navigationMenuRepository;
		}

		public ActionResult AddNavigation(string? Url = null)
		{
			EditNavigationPageModel model = new(Url, new NavigationMenuItem());

			return View(model);
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

			using (NavigationMenuRepository.WriteContext())
			{
				if (!string.IsNullOrWhiteSpace(model.BaseUri))
				{
					NavigationMenuRepository.AddChild(model.BaseUri, model.NavigationMenuItem);
				}
				else
				{
					NavigationMenuRepository.AddOrUpdate(model.NavigationMenuItem);
				}
			}

			return Redirect("/Admin/Navigation/Index");
		}

		public ActionResult DeleteNavigation(int id)
		{
			using (NavigationMenuRepository.WriteContext())
			{
				NavigationMenuItem thisNavigation = NavigationMenuRepository.Find(id) ?? throw new NullReferenceException($"Can not find NavigationMenuItem with id {id}");

				thisNavigation.DateDeleted = DateTime.Now;
			}

			return Redirect("/Admin/Navigation/Index");
		}

		public ActionResult EditNavigation(string Url)
		{
			EditNavigationPageModel model = new(Url, NavigationMenuRepository.GetByUri(Url));

			return model.NavigationMenuItem is null || !SecurityProvider.CheckAccess(model.NavigationMenuItem, PermissionTypes.Write)
				? Content("")
				: View(model);
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

			using (NavigationMenuRepository.WriteContext())
			{
				NavigationMenuItem thisNavigation = NavigationMenuRepository.Find(model.NavigationMenuItem);

				if (SecurityProvider.CheckAccess(thisNavigation, PermissionTypes.Write))
				{
					thisNavigation.Href = model.NavigationMenuItem.Href;
					thisNavigation.Name = model.NavigationMenuItem.Name;
					thisNavigation.Text = model.NavigationMenuItem.Text;
					thisNavigation.Icon = model.NavigationMenuItem.Icon;

					NavigationMenuRepository.AddOrUpdate(thisNavigation);
				}
			}

			return Redirect("/Admin/Navigation/Index");
		}

	}
}