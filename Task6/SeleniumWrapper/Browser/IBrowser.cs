using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumWrapper.Elements;

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
        event Action<string> WindowChanged;
        event Action<string> WindowClosed;
        event Action BrowserClosed;
        event Action BrowserOpened;

        IBrowserWindow Window{ get; }
        void NewWindow(string url);
        void NewWindow();
        void CloseWindow(string windowHandle);
        void SwitchToWindow(string windowHandle);

        IMouseActions MouseActions { get; }
        IKeyActions KeyActions { get; }
        ICookieManager Cookie { get; }
    }

    public interface ICookieManager
    {
        ReadOnlyCollection<Cookie> AsReadonly();
        void Add(Cookie cookie);
        void Clear();
        void Delete(Cookie cookie);
        void Delete(string name);
        Cookie this[string name] { get; }
    }

    public interface IMouseActions  : IAction
    {
        IMouseActions Click();
        IMouseActions Click(BaseElement onElement);
        IMouseActions ClickAndHold(BaseElement onElement);
        IMouseActions ClickAndHold();
        IMouseActions ContextClick();
        IMouseActions ContextClick(BaseElement onElement);
        IMouseActions DoubleClick();
        IMouseActions DoubleClick(BaseElement onElement);
        IMouseActions DragAndDrop(BaseElement source, BaseElement target);
        IMouseActions DragAndDropToOffset(BaseElement source, int offsetX, int offsetY);
        IMouseActions MoveByOffset(int offsetX, int offsetY);
        IMouseActions MoveToElement(BaseElement toElement);
        IMouseActions MoveToElement(BaseElement toElement, int offsetX, int offsetY);
        IMouseActions MoveToElement(BaseElement toElement, int offsetX, int offsetY, MoveToElementOffsetOrigin offsetOrigin);
        IMouseActions Release(BaseElement onElement);
        IMouseActions Release();
        void Reset();
    }

    public interface IKeyActions : IAction
    {
        IKeyActions KeyDown(string theKey);
        IKeyActions KeyDown(BaseElement element, string theKey);
        IKeyActions KeyUp(BaseElement element, string theKey);
        IKeyActions KeyUp(string theKey);
        IKeyActions SendKeys(BaseElement element, string keysToSend);
        IKeyActions SendKeys(string keysToSend);
        void Reset();
    }

}