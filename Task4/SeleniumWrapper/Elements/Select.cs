using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium.Support.UI;
using SeleniumWrapper.Browser;

namespace SeleniumWrapper.Elements
{
    public sealed partial class Select : BaseElement
    {
        public Select(WebElementKeeper elementKeeper) : base(elementKeeper)
        {
        }

        private SelectElement select;
        private SelectElement DropDownManager 
        {
            get
            {
                if(select == null)
                {
                    select = new SelectElement(Element);
                }
                return select;
            }
        }
        public string AccessKey => Element.GetAttribute("accesskey");
        public string Form => Element.GetAttribute("form");

#region Wrapper for SelectElement
        public bool IsMultiple => DropDownManager.IsMultiple;
        public ReadOnlyCollection<Option> Options 
        {
            get
            {
                return DropDownManager.Options.Select(x=>new Option(x as WebElementKeeper)).ToList().AsReadOnly();
            }
        }
        public Option SelectedOption => new Option(DropDownManager.SelectedOption as WebElementKeeper);
        public ReadOnlyCollection<Option> AllSelectedOptions
        {
            get 
            {
                return DropDownManager.Options.Select(x=>new Option(x as WebElementKeeper)).ToList().AsReadOnly();
            }
        }
        
        public void DeselectAll()=>DropDownManager.DeselectAll();
        public void DeselectByIndex(int index) => DropDownManager.DeselectByIndex(index);
        public void DeselectByText(string text) => DropDownManager.DeselectByText(text);
        public void DeselectByValue(string value) => DropDownManager.DeselectByValue(value);
        public void SelectByIndex(int index) => DropDownManager.SelectByIndex(index);
        public void SelectByText(string text, bool partialMatch = false) => DropDownManager.SelectByText(text,partialMatch);
        public void SelectByValue(string value) => DropDownManager.SelectByValue(value);
#endregion

    }
}