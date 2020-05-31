

using System;
using System.Collections.ObjectModel;

namespace SeleniumWrapper
{
    interface IBrowser : IDisposable
    {
        void Quit();
        string BrowserName { get; }

        ReadOnlyCollection<IBrowserWindow> Windows{ get; }

        IBrowserWindow NewWindow(string url);
        IBrowserWindow NewWindow();
        void CloseWindow(string windowHandle);
        IBrowserWindow SwitchToWindow(string windowHandle);
    }

}