using System;
using System.Collections.ObjectModel;

using OpenQA.Selenium;
using SeleniumWrapper;

namespace Task_3.Pages
{
    class PageBase
    {
        protected PageBase(IBrowser browser, string pageUrl)
        {
            if(browser == null)
            {
                throw new ArgumentException("Browser doesn`t setted!");
            }

            this.browser = browser;

            if(browser.Window == null)
            {
                browser.NewWindow(pageUrl);
            }
            else if(browser.Window.Url != pageUrl)
            {
                browser.Window.Url = pageUrl;
            }

            MyHandle = browser.Window.Handle;

            browser.WindowChanged += WindowChanged;
            browser.BrowserClosed += BrowserClosed;
        }

        ~PageBase()
        {
            browser.WindowChanged -= WindowChanged;
            browser.BrowserClosed -= BrowserClosed;
        }

        protected readonly IBrowser browser;
        public string MyHandle {get;}
        public string Title 
        {
            get
            {
                Check();
                return browser.Window.Title;
            }
        }

        protected bool WindowChangedTougle { get; private set; } = false;
        private void WindowChanged(IBrowser browser)
        {
            WindowChangedTougle = browser.Window.Handle != MyHandle;
        }
        protected bool BrowserClosedTougle { get; private set;} = false;
        private void BrowserClosed(IBrowser browser)
        {
            BrowserClosedTougle = true;
        }
    
        protected void Check()
        {
            if(BrowserClosedTougle)
            {
                throw new Exception("Browser was closed");
            }

            if(WindowChangedTougle)
            {
                if(browser.OpenedWindows.Contains(MyHandle))
                {
                    browser.SwitchToWindow(MyHandle);
                }
                else
                {
                    throw new Exception("Page was closed!");
                }
            }
        }

        protected ReadOnlyCollection<IWebElement> FindElements(string xPath, BrowserWait wait = null)
        {
            Check();
                        
            return (wait == null ? browser.Window.FindElements(By.XPath(xPath)) :  
                    wait.Until(x=>
                    {
                        var data = x.Window.FindElements(By.XPath(xPath));
                        return (data.Count == 0 ? null : data); 
                    }));
        } 
    }
}