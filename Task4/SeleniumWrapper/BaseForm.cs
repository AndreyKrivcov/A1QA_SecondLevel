using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Elements;

namespace SeleniumWrapper
{
    public abstract class BaseForm
    {
        protected BaseForm(IBrowser browser, string url, bool openNewWindow)
        {
            this.browser = browser;

            if(openNewWindow)
            {
                browser.NewWindow(url);
            }
            else
            {
                browser.Window.Url = url;
            }

            Handle = browser.Window.Handle;

            browser.WindowChanged += WindowChanged;
            browser.BrowserClosed += BrowserClosed;
            browser.WindowClosed += WindowClosed;
        }

        ~BaseForm()
        {
            browser.WindowChanged -= WindowChanged;
            browser.BrowserClosed -= BrowserClosed;
            browser.WindowClosed -= WindowClosed;
        }

        protected readonly IBrowser browser;

#region Properties
        public string Handle { get; }
        public bool WasClosed => browserClosedTougle || windowClosedTougle;
        public string Title
        {
            get
            {
                CheckWindow();
                return browser.Window.Title;
            }
        }
#endregion

#region Callbacks
        private bool windowChangedTougle = false;
        private void WindowChanged(string newHandle)
        {
            windowChangedTougle = newHandle != Handle;
        }
        private bool browserClosedTougle = false;
        private void BrowserClosed()
        {
            browserClosedTougle = true;
        }

        private bool windowClosedTougle = false;
        private void WindowClosed(string handle)
        {
            windowClosedTougle = handle == Handle;
        }
#endregion

        protected void CheckWindow()
        {
            if(WasClosed)
            {
                throw new Exception("Widow was closed");
            }

            if(windowChangedTougle)
            {
                browser.SwitchToWindow(Handle);
            }
        }


#region Wait
        protected BaseElement WaitForElement(By by, TimeSpan timeout, TimeSpan? sleepInterval = null, params Type[] ignoringExceptions)
        {
            if(ignoringExceptions != null && 
               ignoringExceptions.Contains(typeof(NoSuchElementException)))
            {
                ignoringExceptions = ignoringExceptions.Concat(new [] { typeof(NoSuchElementException)}).ToArray();
            }
            return Wait(timeout, (IBrowser b)=> b.Window.FindElement(by) , sleepInterval, ignoringExceptions);
        }

        protected ElementsKeeper WaitForElements(By by, TimeSpan timeout, TimeSpan? sleepInterval = null, params Type[] ignoringExceptions)
        {
            if(ignoringExceptions != null && 
               ignoringExceptions.Contains(typeof(NoSuchElementException)))
            {
                ignoringExceptions = ignoringExceptions.Concat(new [] { typeof(NoSuchElementException)}).ToArray();
            }

            ElementsKeeper elements = null;
            Wait(timeout, (IBrowser b)=>
            {
                elements = b.Window.FindElements(by);
                foreach (var item in elements.Elements)
                {
                    if(!item.IsExists)
                    {
                        return false;
                    }
                }

                return true;
            },sleepInterval,ignoringExceptions);

            return elements;
        }

        protected T Wait<T>(TimeSpan timeout, Func<IBrowser, T> f, TimeSpan? sleepInterval = null, params Type[] ignoringExceptions)
        {
            var wait = sleepInterval.HasValue ? new BrowserWait(new SystemClock(),browser,timeout, sleepInterval.Value) 
                                              : new BrowserWait(browser,timeout);

            if(ignoringExceptions != null && ignoringExceptions.Count() > 0)
            {
                wait.IgnoreExceptionTypes(ignoringExceptions);
            }

            return wait.Until(x=> f(x));
        }
#endregion

    }
}