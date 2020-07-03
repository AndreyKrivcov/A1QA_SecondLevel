using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumWrapper.Browser;

namespace SeleniumWrapper.Utils
{
    /// <summary>
    /// Provides the ability to wait for an arbitrary condition during test execution.
    /// Class implementation is the same as WebDriverWait implementation and was taken from 
    /// Selenium github https://github.com/SeleniumHQ/selenium/blob/07a18746ff756e90fd79ef253a328bd7dfa9e6dc/dotnet/src/webdriver/Support/WebDriverWait.cs
    /// </summary>
    /// <example>
    /// <code>
    /// IWait wait = new BrowserWait(browser, TimeSpan.FromSeconds(3))
    /// IWebElement element = wait.Until(BrowserWait => BrowserWait.Window.FindElement(By.Name("q")));
    /// </code>
    /// </example>
    public class BrowserWait : DefaultWait<IBrowser>
    {
        public BrowserWait(IBrowser browser, TimeSpan timeout)
            : this(new SystemClock(), browser, timeout, DefaultSleepTimeout)
        {
        }
       
        public BrowserWait(IClock clock, IBrowser browser, TimeSpan timeout, TimeSpan sleepInterval)
            : base(browser, clock)
        {
            this.Timeout = timeout;
            this.PollingInterval = sleepInterval;
            this.IgnoreExceptionTypes(typeof(NotFoundException));
        }

        private static TimeSpan DefaultSleepTimeout
        {
            get { return TimeSpan.FromMilliseconds(500); }
        } 

        private static IBrowser Browser
        {
            get
            {
                IBrowser browser = SeleniumWrapper.Browser.Browser.Instance();
                if(!browser.IsOpened)
                {
                    throw new Exception("Browser closed");
                }
                return browser;
            }
        } 

        public static T Wait<T>(TimeSpan timeout, Func<IBrowser, T> f, TimeSpan? sleepInterval = null, params Type[] ignoringExceptions)
        {
            var wait = sleepInterval.HasValue ? new BrowserWait(new SystemClock(),Browser,timeout, sleepInterval.Value) 
                                              : new BrowserWait(Browser,timeout);

            if(ignoringExceptions != null && ignoringExceptions.Count() > 0)
            {
                wait.IgnoreExceptionTypes(ignoringExceptions);
            }

            return wait.Until(x=> f(x));
        }

    }

    public static class CollectionWaitExtention
    {
        public static IEnumerable<T> WaitForElements<T>(this IEnumerable<T> elements, TimeSpan timeout, TimeSpan? sleepInterval = null, params Type[] ignoringExceptions)
            where T : Elements.BaseElement
        {
            if(ignoringExceptions != null && 
               ignoringExceptions.Contains(typeof(NoSuchElementException)))
            {
                ignoringExceptions = ignoringExceptions.Concat(new [] { typeof(NoSuchElementException)}).ToArray();
            }
            
            BrowserWait.Wait(timeout,browser=>
            {
                foreach (var item in elements)
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
    }
}