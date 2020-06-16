using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumWrapper.Elements;

namespace SeleniumWrapper.Browser
{ 
    public class KeyUtils : IAction
    {
        public KeyUtils(IWebDriver driver)
        {
            actions = new OpenQA.Selenium.Interactions.Actions(driver);
        }
        private readonly OpenQA.Selenium.Interactions.Actions actions;

        public KeyUtils KeyDown(string theKey)
        {
            actions.KeyDown(theKey);
            return this;
        }
        public KeyUtils KeyDown(BaseElement element, string theKey)
        {
            actions.KeyDown(element.GetIWebElement(),theKey);
            return this;
        }
        public KeyUtils KeyUp(BaseElement element, string theKey)
        {
            actions.KeyUp(element.GetIWebElement(), theKey);
            return this;
        }
        public KeyUtils KeyUp(string theKey)
        {
            actions.KeyUp(theKey);
            return this;
        }
        public KeyUtils SendKeys(BaseElement element, string keysToSend)
        {
            actions.SendKeys(element.GetIWebElement(),keysToSend);
            return this;
        }
        public KeyUtils SendKeys(string keysToSend)
        {
            actions.SendKeys(keysToSend);
            return this;
        }
        public void Perform() => actions.Perform();
        public void Reset() => actions.Reset();
    }
}