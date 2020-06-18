
using System;
using System.Drawing;

using OpenQA.Selenium;
using SeleniumWrapper.Elements;

namespace SeleniumWrapper.Browser
{
    public class BrowserWindow : IBrowserWindow
    {
        internal BrowserWindow(){}
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

        public T FindElement<T>(By by) where T : BaseElement => new DefaultElement<T>(by,-1,null);

        public ElementsKeeper<T> FindElements<T>(By by) where T : BaseElement => 
            new ElementsKeeper<T>(by);

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
        public void WaitForLoading()
        {
            DriverKeeper.GetDriver.JavaScriptExecutor.ExecuteScript("window.onload");
        }
    }
}