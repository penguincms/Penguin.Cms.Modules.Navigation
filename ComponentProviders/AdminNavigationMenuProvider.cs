using Penguin.Cms.Modules.Core.ComponentProviders;
using Penguin.Cms.Modules.Core.Navigation;
using Penguin.Cms.Modules.Navigation.Constants.Strings;
using Penguin.Navigation.Abstractions;
using Penguin.Security.Abstractions;
using Penguin.Security.Abstractions.Interfaces;
using System.Collections.Generic;
using SecurityRoles = Penguin.Security.Abstractions.Constants.RoleNames;

namespace Penguin.Cms.Modules.Navigation.ComponentProviders
{
    public class AdminNavigationMenuProvider : NavigationMenuProvider
    {
        public override INavigationMenu GenerateMenuTree()
        {
            return new NavigationMenu()
            {
                Name = "Admin",
                Text = "Admin",
                Children = new List<INavigationMenu>() {
                        new NavigationMenu()
                        {
                            Text = "Navigation",
                            Name = "NavigationAdmin",
                            Href = "/Admin/Navigation/Index",
                            Permissions = new List<ISecurityGroupPermission>()
                            {
                                CreatePermission(RoleNames.CONTENT_MANAGER, PermissionTypes.Read),
                                CreatePermission(SecurityRoles.SYS_ADMIN, PermissionTypes.Read | PermissionTypes.Write)
                            }
                        },
                    }
            };
        }
    }
}