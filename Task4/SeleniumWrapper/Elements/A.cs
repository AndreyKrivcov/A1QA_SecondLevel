using System;
using OpenQA.Selenium;

namespace SeleniumWrapper.Elements
{
    public sealed class A : BaseElement
    {
        public A(Func<IWebElement> elementFinder, IWebDriver driver) : base(elementFinder, driver)
        {
        }
        public string AccessKey => Element.GetAttribute("accesskey");
        public string Coords => Element.GetAttribute("coords");
        
    }
}