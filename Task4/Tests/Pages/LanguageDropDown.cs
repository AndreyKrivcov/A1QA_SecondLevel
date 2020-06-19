using System;
using System.Linq;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Elements;

namespace Tests.Pages
{
    class LanguageDropDown
    {
        public LanguageDropDown(BaseElement languageDD, Action<string> logger, IBrowser browser, TimeSpan timeout)
        {
            this.logger = logger;
            languageDD.Click();
            elements = GetLanguages(browser,timeout);
        }

        private readonly string DDMenuSelector = "//div[@id=\"global_actions\"]//div[@class=\"popup_body popup_menu\"]";
        private readonly ReadOnlyCollection<A> elements;
        private readonly Action<string> logger;

        public ReadOnlyCollection<LanguageItem> Items => 
            elements.Select(x=>new LanguageItem(x,logger)).ToList().AsReadOnly();

        private ReadOnlyCollection<A> WaitForElements(IBrowser b, TimeSpan timeout)
        {
            BrowserWait wait = new BrowserWait(b,timeout);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            Div contaner = wait.Until(x=>
            {
                Div div = b.Window.FindElement<Div>(By.XPath(DDMenuSelector));
                return ((!div.Disabled && !div.Disabled && div.Disabled) ? div : null);
            });

            return contaner.FindElements<A>(By.TagName("a"));
        }

        private ReadOnlyCollection<A> GetLanguages(IBrowser b, TimeSpan timeout)
        {
            bool tryToWait = true;
            try
            {
                Div contaner = b.Window.FindElement<Div>(By.XPath(DDMenuSelector));
                if(contaner.Displayed)
                {
                    return contaner.FindElements<A>(By.TagName("a"));
                }
                else
                {
                    tryToWait = false;
                    return WaitForElements(b,timeout);
                }
            }
            catch(Exception e)
            {
                if(!tryToWait)
                {
                    throw e;
                }
                else
                {
                    return WaitForElements(b,timeout);
                }
            }
        }
    }

    class LanguageItem
    {
        public LanguageItem(A a, Action<string> logger)
        {

            this.logger = logger;
            this.a = a;
        }
        private readonly A a;
        Action<string> logger;
        public string Name => a.InnerHTML; 
        
        public void Click()
        {
            a.Click();
            logger($"Language changed to {a.InnerHTML}");
        }
    }
}