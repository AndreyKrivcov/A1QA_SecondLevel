using System;
using OpenQA.Selenium;

namespace SeleniumWrapper.Elements
{
    public sealed class Option : BaseElement
    {
        public Option(Func<IWebElement> elementFinder, IWebDriver driver) : base(elementFinder, driver)
        {
            CheckTag("option");
        }

        public string Label => Element.GetAttribute("label");
        public string Value => Element.GetAttribute("value");
        public string InnerHTML => this;
        public static implicit operator string(Option o)
        {
            return o.Element.GetAttribute("innerHTML");
        }        
    }
}