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
            dropDownManager = new SelectElement(Element);
        }

        private readonly SelectElement dropDownManager;
        public string AccessKey => Element.GetAttribute("accesskey");
        public string Form => Element.GetAttribute("form");

#region Wrapper for SelectElement
        public bool IsMultiple => dropDownManager.IsMultiple;
        public ReadOnlyCollection<Option> Options 
        {
            get
            {
                return dropDownManager.Options.Select(x=>new Option(x as WebElementKeeper)).ToList().AsReadOnly();
            }
        }
        public Option SelectedOption => new Option(dropDownManager.SelectedOption as WebElementKeeper);
        public ReadOnlyCollection<Option> AllSelectedOptions
        {
            get 
            {
                return dropDownManager.Options.Select(x=>new Option(x as WebElementKeeper)).ToList().AsReadOnly();
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