
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
            throw new NotImplementedException();
        }
        
        public string Url { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Title => throw new NotImplementedException();

        public string Handle => throw new NotImplementedException();

        public Point Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Size Size { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Back()
        {
            throw new NotImplementedException();
        }

        public IWebElement FindElement(By by)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            throw new NotImplementedException();
        }

        public void Forward()
        {
            throw new NotImplementedException();
        }

        public void FullScreen()
        {
            throw new NotImplementedException();
        }

        public void GoToUrl(string url)
        {
            throw new NotImplementedException();
        }

        public void GoToUrl(Uri url)
        {
            throw new NotImplementedException();
        }

        public void Maximize()
        {
            throw new NotImplementedException();
        }

        public void Minimize()
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}