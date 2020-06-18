using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWrapper.Elements
{
    public sealed partial class Select : BaseElement
    {
        public Select(By by, int ind, BaseElement parentElemen) : base(by, ind, parentElemen)
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
        public ReadOnlyCollection<Option> Options 
        {
            get
            {
                var data = dropDownManager.Options;
                List<Option> ans = new List<Option>();
                for (int i = 0; i < data.Count; i++)
                {
                    ans.Add(new Option(By,i,false));
                }

                return ans.AsReadOnly();
            }
        }
        public Option SelectedOption => new Option(By,-1,false);
        public ReadOnlyCollection<Option> AllSelectedOptions
        {
            get
            {
                var data = dropDownManager.AllSelectedOptions;
                List<Option> ans = new List<Option>();
                for (int i = 0; i < data.Count; i++)
                {
                    ans.Add(new Option(By,i,true));
                }

                return ans.AsReadOnly();
            }
        }
        
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