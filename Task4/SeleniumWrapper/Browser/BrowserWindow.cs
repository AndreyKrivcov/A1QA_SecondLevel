
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumWrapper.Elements;

namespace SeleniumWrapper.Browser
{
    internal class BrowserWindow : IBrowserWindow
    {
        public BrowserWindow(){}
        public string Url 
        { 
            get => DriverKeeper.GetDriver.Url; 
            set => DriverKeeper.GetDriver.Url = value; 
        }

        public string Title => DriverKeeper.GetDriver.Title;

        public string Handle => DriverKeeper.GetDriver.CurrentWindowHandle;

        public Point Position 
        { 
            get => DriverKeeper.GetDriver.Manage().Window.Position; 
            set => DriverKeeper.GetDriver.Manage().Window.Position = value; 
        }
        public Size Size 
        { 
            get => DriverKeeper.GetDriver.Manage().Window.Size; 
            set => DriverKeeper.GetDriver.Manage().Window.Size = value; 
        }

        public void Back() => DriverKeeper.GetDriver.Navigate().Back(); 

        public T FindElement<T>(By by) where T : BaseElement
        {
            return new DefaultElement<T>(DriverKeeper.GetDriver.FindElement(by) as WebElementKeeper);
        }

        public ReadOnlyCollection<T> FindElements<T>(By by) where T : BaseElement
        { 
            var elements = DriverKeeper.GetDriver.FindElements(by)
                .Select(x=>new DefaultElement<T>(x as WebElementKeeper));
            return elements.ToElementArray().AsReadOnly();
        }

        public void Forward() => DriverKeeper.GetDriver.Navigate().Forward();

        public void FullScreen() => DriverKeeper.GetDriver.Manage().Window.FullScreen();

        public void GoToUrl(string url) => DriverKeeper.GetDriver.Navigate().GoToUrl(url);

        public void GoToUrl(Uri url) => DriverKeeper.GetDriver.Navigate().GoToUrl(url);

        public void Maximize() => DriverKeeper.GetDriver.Manage().Window.Maximize();
        
        public void Minimize() => DriverKeeper.GetDriver.Manage().Window.Minimize();

        public void Refresh() => DriverKeeper.GetDriver.Navigate().Refresh();

        public void Scroll(int x, int y)
        {
            DriverKeeper.GetDriver.JavaScriptExecutor.ExecuteScript($"window.scrollBy({x},{y})");
        }
        public void WaitForLoading(TimeSpan timeout, TimeSpan? sleep = null, params Type[] ignoringExceptions)
        {
            WebDriverWait wait = (sleep.HasValue ? new WebDriverWait(new SystemClock(), DriverKeeper.GetDriver,timeout, sleep.Value)
                                                 : new WebDriverWait(DriverKeeper.GetDriver, timeout));
            
            if(ignoringExceptions != null && ignoringExceptions.Length >0)
            {
                wait.IgnoreExceptionTypes(ignoringExceptions);
            }

            wait.Until((IWebDriver driver)=>
            {
                return DriverKeeper.GetDriver.JavaScriptExecutor
                    .ExecuteScript("return document.readyState").Equals("complete");
            });
        }
        public T WaitForElement<T>(By by, TimeSpan timeout, TimeSpan? sleepInterval, params Type[] ignoringExceptions) where T : BaseElement
        {
            if(ignoringExceptions != null && 
               ignoringExceptions.Contains(typeof(NoSuchElementException)))
            {
                ignoringExceptions = ignoringExceptions.Concat(new [] { typeof(NoSuchElementException)}).ToArray();
            }
            return BrowserWait.Wait(timeout, (IBrowser b)=> 
            {
                var element = b.Window.FindElement<T>(by);
                return (element.IsExists ? element : null);
            }, sleepInterval, ignoringExceptions);
        }

        public ReadOnlyCollection<T> WaitForElements<T>(By by, TimeSpan timeout, TimeSpan? sleepInterval, params Type[] ignoringExceptions)
                                    where T : BaseElement
        {
            if(ignoringExceptions != null && 
               ignoringExceptions.Contains(typeof(NoSuchElementException)))
            {
                ignoringExceptions = ignoringExceptions.Concat(new [] { typeof(NoSuchElementException)}).ToArray();
            }

            ReadOnlyCollection<T> elements = null;
            BrowserWait.Wait(timeout, (IBrowser b)=>
            {
                elements = b.Window.FindElements<T>(by);
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