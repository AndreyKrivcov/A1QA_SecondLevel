using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumWrapper.Browser;

namespace SeleniumWrapper.Elements
{
    public sealed partial class Select
    {
        public sealed class Option : BaseElement
        {
            public Option(WebElementKeeper elementKeeper) : base(elementKeeper)
            {
            }
            public string Label => Element.GetAttribute("label");
            public string Value => Element.GetAttribute("value");
            public static implicit operator string(Option o)
            {
                return o.InnerHTML;
            }        
        }
    }
}