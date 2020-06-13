using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWrapper.Browser
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
    }
}