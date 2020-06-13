using System;
using System.Collections.ObjectModel;

namespace SeleniumWrapper.Browser
{
    public interface IBrowser : IDisposable
    {
        void Quit();
        string BrowserName { get; }
        string Version { get; }
        bool IsOpened { get; }

        ReadOnlyCollection<string> OpenedWindows { get; }

        event Action<IBrowser> WindowChanged;
        event Action<IBrowser, string> WindowClosed;
        event Action<IBrowser> BrowserClosed;

        IBrowserWindow Window{ get; }
        void NewWindow(string url);
        void NewWindow();
        void CloseWindow(string windowHandle);
        void SwitchToWindow(string windowHandle);
    }

}