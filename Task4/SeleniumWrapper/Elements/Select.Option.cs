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
            public Option(By by, int ind, bool isAllSelected) : base(by,ind, null)
            {
                this.isAllSelected = isAllSelected;
               // CheckTag("option");
            }

            private readonly bool isAllSelected;

            protected override void GetElement(int counter = -1, bool force = false)
            {
                if(element == null || force)
                {
                    if(counter++ > 10)
                    {
                        throw new TimeoutException("Can`t create new element");
                    }

                    try
                    {
                        var select = new SelectElement(DriverKeeper.GetDriver.FindElement(By));
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
                    }
                    catch(Exception e)
                    {
                        if(counter > 10)
                        {
                            throw e;
                        }   
                    }

                    if(counter > 0)
                    {
                        System.Threading.Thread.Sleep(6000);
                        GetElement(counter, false);
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