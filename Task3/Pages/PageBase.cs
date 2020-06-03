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
            this.pageUrl = pageUrl;

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
        protected readonly string pageUrl;
        public string MyHandle {get;}

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
            
            if(browser.Window.Url != pageUrl)
            {
                throw new Exception($"Url adress is {browser.Window.Url} but expected {pageUrl}");
            }
        }

        protected ReadOnlyCollection<IWebElement> FindElements(string xPath, BrowserWait wait = null)
        {
            Check();
                        
            return (wait == null ? browser.Window.FindElements(By.XPath(xPath)) :  
                    wait.Until(x => x.Window.FindElements(By.XPath(xPath))));
        } 
    }
}