using OpenQA.Selenium;

namespace SeleniumWrapper
{
    interface IBrowserWindow : IWindow, ISearchContext, INavigation
    {
        string Url { get; set; }
        string Title { get; }
        string Handle { get; }
    } 
}