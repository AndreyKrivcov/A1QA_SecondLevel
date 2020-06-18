using System.Collections.ObjectModel;
using System.Linq;

using OpenQA.Selenium;
using SeleniumWrapper;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Elements;

namespace Tests.Pages
{
    class InstallSteam : BaseForm
    {
        public InstallSteam(IBrowser browser, A link) : base(browser)
        {
            link.Click();
            this.Url = browser.Window.Url;
        }

        private readonly string InstallBtn = "//a[@class=\"about_install_steam_link\"]";

        public void Install()
        {
            ReadOnlyCollection<A> buttons = browser.Window.FindElements<A>(By.XPath(InstallBtn));
            buttons.First().Click();
        }
    }
}