using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace SeleniumWrapper.Browser
{
    public interface IBrowser : IDisposable
    {
        void Quit();
        string BrowserName { get; }
        string Version { get; }
        bool IsOpened { get; }

        ReadOnlyCollection<string> OpenedWindows { get; }
        IJavaScriptExecutor JavaScriptExecutor { get; }
        MouseUtils MouseUtils { get; }
        KeyUtils KeyUtils { get; }

        event Action<string> WindowChanged;
        event Action<string> WindowClosed;
        event Action BrowserClosed;
        event Action BrowserOpened;

        IBrowserWindow Window{ get; }
        void NewWindow(string url);
        void NewWindow();
        void CloseWindow(string windowHandle);
        void SwitchToWindow(string windowHandle);
    }

}