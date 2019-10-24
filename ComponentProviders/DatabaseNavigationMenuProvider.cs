using Penguin.Cms.Abstractions.Interfaces;
using Penguin.Cms.Navigation.Repositories;
using Penguin.DependencyInjection.Abstractions.Interfaces;
using Penguin.Navigation.Abstractions;
using System.Collections.Generic;

namespace Penguin.Cms.Modules.Navigation.ComponentProviders
{
    public class DatabaseNavigationMenuProvider : IProvideComponents<INavigationMenu, string>, ISelfRegistering
    {
        protected NavigationMenuRepository NavigationMenuRepository { get; set; }

        public DatabaseNavigationMenuProvider(NavigationMenuRepository navigationMenuRepository)
        {
            NavigationMenuRepository = navigationMenuRepository;
        }

        public IEnumerable<INavigationMenu> GetComponents(string MenuName)
        {
            yield return NavigationMenuRepository.GetByName(MenuName);
        }
    }
}