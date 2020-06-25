
using System.Collections.ObjectModel;
using System.Collections.Generic;

using OpenQA.Selenium;

using System.Linq;
using System;
using SeleniumWrapper.Utils;

namespace SeleniumWrapper.Browser
{
    internal partial class Browser : IBrowser
    {    
        private Browser(){}
        private static Browser instance;
        public static IBrowser Instance()
        {
            if(instance == null)
            {
                instance = new Browser();
            }
            return instance;
        }
        public string BrowserName => (DriverKeeper.GetDriver == null ? null : DriverKeeper.GetDriver.BrowserName);
        public string Version => (DriverKeeper.GetDriver == null ? null : DriverKeeper.GetDriver.Version);
        public bool IsOpened => DriverKeeper.GetDriver.IsOpened;
        public IJavaScriptExecutor JavaScriptExecutor => DriverKeeper.GetDriver.JavaScriptExecutor;
        public ReadOnlyCollection<string> OpenedWindows => 
            (IsOpened ? new List<string>().AsReadOnly() : DriverKeeper.GetDriver.WindowHandles);

        public override int GetHashCode()
        {
            return DriverKeeper.GetDriver.GetHashCode();
        }

#region  Window keeper
        private IBrowserWindow myWindow;
        public IBrowserWindow Window 
        { 
            get
            {
                if(myWindow == null || !IsOpened)
                {
                    OpenNewWindowTab(null);
                }
                return myWindow;
            } 
        } 
#endregion
#region Actions
        public IKeyActions KeyActions => new KeyActionsManager();
        public IMouseActions MouseActions => new MouseActionsManager();
#endregion
        public event Action<string> WindowChanged;
        public event Action<string> WindowClosed;
        public event Action BrowserClosed;
        public event Action BrowserOpened;
        
        public void CloseWindow(string windowHandle)
        {
            if(!IsOpened)
            {
                return;
            }
            if(!OpenedWindows.Contains(windowHandle))
            {
                throw new ArgumentException($"Can`t find window with handle = {windowHandle}");
            }
            if(OpenedWindows.Count == 1)
            {
                Quit();
                return;
            }

            string currentHandle = DriverKeeper.GetDriver.CurrentWindowHandle;
            
            bool windowChanged = false;
            if(currentHandle != windowHandle)
            {
                DriverKeeper.GetDriver.SwitchTo().Window(windowHandle).Close();
                DriverKeeper.GetDriver.SwitchTo().Window(currentHandle);
            }
            else
            {
                DriverKeeper.GetDriver.Close();
                DriverKeeper.GetDriver.SwitchTo().Window(OpenedWindows.Last());
                windowChanged = true;
            }

            WindowClosed?.Invoke(windowHandle);
            if(windowChanged)
            {
                WindowChanged?.Invoke(OpenedWindows.Last());
            }
        }

        public void Dispose()
        {
            if(IsOpened)
            {
                DriverKeeper.GetDriver.Dispose();
                BrowserClosed?.Invoke();
            }
        }

        public void NewWindow(string url)
        {
            OpenNewWindowTab(url);
        }

        public void NewWindow()
        {
            OpenNewWindowTab(null);
        }

        protected virtual void OpenNewWindowTab(string url)
        {
            bool shouldOpen = !IsOpened;            
           
            if(!shouldOpen)
            {
                JavaScriptExecutor.ExecuteScript("window.open();");
                DriverKeeper.GetDriver.SwitchTo().Window(DriverKeeper.GetDriver.WindowHandles.Last());
            }
            else
            {
                DriverKeeper.GetDriver.SwitchTo().ParentFrame();
            }
            if(!string.IsNullOrEmpty(url) || !string.IsNullOrWhiteSpace(url))
            {
                DriverKeeper.GetDriver.Navigate().GoToUrl(url);
            }

            if(!shouldOpen)
            {
                WindowChanged?.Invoke(DriverKeeper.GetDriver.WindowHandles.Last());
            }
            else
            {
                myWindow = new BrowserWindow();
                BrowserOpened?.Invoke();
            }
        }

        public void Quit()
        {
            if(IsOpened)
            {
                DriverKeeper.GetDriver.Quit();
                BrowserClosed?.Invoke();
            }
        }

        public void SwitchToWindow(string windowHandle)
        {
            if(IsOpened)
            {
                if(!OpenedWindows.Contains(windowHandle))
                {
                    throw new ArgumentException($"Can`t find window with handle = {windowHandle}");
                }

                DriverKeeper.GetDriver.SwitchTo().Window(windowHandle);
                WindowChanged?.Invoke(windowHandle);
            }
        }
    }
}