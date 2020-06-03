using OpenQA.Selenium;

namespace SeleniumWrapper
{
    interface IBrowserWindow : IWindow, ISearchContext, INavigation
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
    } 
}