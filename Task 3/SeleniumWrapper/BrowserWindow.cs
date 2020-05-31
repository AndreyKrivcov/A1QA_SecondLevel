
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using OpenQA.Selenium;

namespace SeleniumWrapper
{
    class BrowserWindow : IBrowserWindow
    {
        public BrowserWindow(IWebDriver webDriver)
        {
            driver = webDriver;
        }
        
        private readonly IWebDriver driver;

        public string Url 
        { 
            get => driver.Url; 
            set => Url = value; 
        }

        public string Title => driver.Title;

        public string Handle => driver.CurrentWindowHandle;

        public Point Position 
        { 
            get => driver.Manage().Window.Position; 
            set => Position = value; 
        }
        public Size Size 
        { 
            get => driver.Manage().Window.Size; 
            set => Size = value; 
        }

        public void Back() => driver.Navigate().Back(); 

        public IWebElement FindElement(By by) => driver.FindElement(by);

        public ReadOnlyCollection<IWebElement> FindElements(By by) => driver.FindElements(by);

        public void Forward() => driver.Navigate().Forward();

        public void FullScreen() => driver.Manage().Window.FullScreen();

        public void GoToUrl(string url) => driver.Navigate().GoToUrl(url);

        public void GoToUrl(Uri url) => driver.Navigate().GoToUrl(url);

        public void Maximize() => driver.Manage().Window.Maximize();
        
        public void Minimize() => driver.Manage().Window.Minimize();

        public void Refresh() => driver.Navigate().Refresh();
    }
}