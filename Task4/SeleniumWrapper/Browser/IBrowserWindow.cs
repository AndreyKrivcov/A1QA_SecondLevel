using System;
using OpenQA.Selenium;
using SeleniumWrapper.Elements;

namespace SeleniumWrapper.Browser
{
    public interface IBrowserWindow : IWindow, INavigation
    {
        /// <summary>
        /// Get current url adress or set new
        /// </summary>
        /// <value>url adress</value>
        string Url { get; set; }
        /// <summary>
        /// Page title
        /// </summary>
        /// <value>Title</value>
        string Title { get; }
        /// <summary>
        /// Window unique handle
        /// </summary>
        /// <value>unique handle</value>
        string Handle { get; }
        T FindElement<T>(By by) where T : BaseElement;
        ElementsKeeper<T> FindElements<T>(By by) where T : BaseElement;

        void Scroll(int x, int y);
        void WaitForLoading(TimeSpan timeout, TimeSpan? sleep = null, params Type[] ignoringExceptions);
    } 
}