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
        BaseElement FindElement(By by);
        ElementsKeeper FindElements(By by);

        void Scroll(int x, int y);
    } 
}