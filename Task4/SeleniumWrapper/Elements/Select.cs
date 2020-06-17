using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWrapper.Elements
{
    public sealed class Select : BaseElement
    {
        public Select(Func<IWebElement> elementFinder, IWebDriver driver) : base(elementFinder, driver)
        {
            CheckTag("select");
            dropDownManager = new SelectElement(Element);
        }

        private readonly SelectElement dropDownManager;
        public string AccessKey => Element.GetAttribute("accesskey");
        public string Form => Element.GetAttribute("form");
        public string Name => Element.GetAttribute("name");

#region Wrapper for SelectElement
        public bool IsMultiple => dropDownManager.IsMultiple;
        public ReadOnlyCollection<Option> Options => dropDownManager.Options
            .Select(x=>new Option(()=>x,driver)).ToList().AsReadOnly();
        public Option SelectedOption => new Option(()=>dropDownManager.SelectedOption, driver);
        public ReadOnlyCollection<Option> AllSelectedOptions => dropDownManager.AllSelectedOptions
            .Select(x=>new Option(()=>x,driver)).ToList().AsReadOnly();

        public void DeselectAll()=>dropDownManager.DeselectAll();
        public void DeselectByIndex(int index) => dropDownManager.DeselectByIndex(index);
        public void DeselectByText(string text) => dropDownManager.DeselectByText(text);
        public void DeselectByValue(string value) => dropDownManager.DeselectByValue(value);
        public void SelectByIndex(int index) => dropDownManager.SelectByIndex(index);
        public void SelectByText(string text, bool partialMatch = false) => dropDownManager.SelectByText(text,partialMatch);
        public void SelectByValue(string value) => dropDownManager.SelectByValue(value);
#endregion

    }
}