using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWrapper.Elements
{
    public sealed partial class Select
    {
        public sealed class Option : BaseElement
        {
            public Option(By by, int ind, bool isAllSelected) : base(by,ind, null)
            {
                this.isAllSelected = isAllSelected;
               // CheckTag("option");
            }

            private readonly bool isAllSelected;

            public override bool IsExists
            {
                get
                {
                    try
                    {
                        element = null;

                        return Wait(TimeSpan.FromMinutes(1),(IWebDriver driver) => 
                        {
                            var select = new SelectElement(driver.FindElement(By));
                            if(ind < 0)
                            {
                                element = select.SelectedOption;
                            }
                            else if(isAllSelected)
                            {
                                var data = select.AllSelectedOptions;
                                element = (data.Count > ind ? data[ind] : null);
                            }
                            else
                            {
                                var data = select.Options;
                                element = (data.Count > ind ? data[ind] : null);
                            }

                            return select != null;
                        });
                    }
                    catch(Exception e)
                    {
                        lastException = e;
                        return false;
                    }
                }
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