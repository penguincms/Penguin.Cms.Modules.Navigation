using Microsoft.AspNetCore.Html;
using Penguin.Cms.Abstractions;
using Penguin.Cms.Abstractions.Interfaces;
using Penguin.Cms.Navigation;
using Penguin.Cms.Navigation.Repositories;
using Penguin.Messaging.Abstractions.Interfaces;
using Penguin.Messaging.Persistence.Messages;
using Penguin.Web.Mvc.Abstractions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Penguin.Cms.Modules.Navigation.Macros
{
    [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
    public class NavigationMenu : IMessageHandler<Updated<NavigationMenuItem>>, IMessageHandler<Created<NavigationMenuItem>>, IMessageHandler<Penguin.Messaging.Application.Messages.Startup>, IMacroProvider
    {
        private static readonly List<Macro> TemplateMacros = new List<Macro>();
        protected NavigationMenuRepository NavigationMenuRepository { get; set; }

        protected IViewRenderService ViewRenderService { get; set; }

        public NavigationMenu(IViewRenderService viewRenderService, NavigationMenuRepository navigationMenuRepository)
        {
            this.ViewRenderService = viewRenderService;
            this.NavigationMenuRepository = navigationMenuRepository;
        }

        public void AcceptMessage(Updated<NavigationMenuItem> page)
        {
            this.Refresh();
        }

        public void AcceptMessage(Created<NavigationMenuItem> page)
        {
            this.Refresh();
        }

        public void AcceptMessage(Penguin.Messaging.Application.Messages.Startup startup)
        {
            this.Refresh();
        }

        public List<Macro> GetMacros(object o)
        {
            return TemplateMacros;
        }

        [SuppressMessage("Design", "CA1054:Uri parameters should not be strings")]
        public HtmlString Render(string Uri)
        {
            NavigationMenuItem toRender = this.NavigationMenuRepository.GetByUri(Uri);

            if (toRender is null)
            {
                return new HtmlString($"NavigationMenu with uri {Uri} was not found");
            }
            else
            {
                Task<string> result = this.ViewRenderService.RenderToStringAsync("~/Views/Shared/Components/NavigationMenu/Default.cshtml", "", toRender, true);

                result.Wait();

                return new HtmlString(result.Result);
            }
        }

        private static Macro NavToMacro(NavigationMenuItem thisNav)
        {
            Macro macro = new Macro
            ("Navigation",
                 $"@NavigationMenu.Render(\"{thisNav.Uri}\")"

            );

            return macro;
        }

        private void Refresh()
        {
            lock (TemplateMacros)
            {
                TemplateMacros.Clear();

                IEnumerable<NavigationMenuItem> allNavs = this.NavigationMenuRepository;

                foreach (NavigationMenuItem thisNav in allNavs)
                {
                    TemplateMacros.Add(NavToMacro(thisNav));
                }
            }
        }
    }
}