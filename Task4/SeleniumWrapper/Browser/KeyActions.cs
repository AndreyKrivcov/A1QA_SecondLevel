using SeleniumWrapper.Elements;

namespace SeleniumWrapper.Browser
{ 
    internal partial class Browser
    {
        private sealed class KeyActionsManager : IKeyActions
        {            
            private readonly OpenQA.Selenium.Interactions.Actions actions = 
                new OpenQA.Selenium.Interactions.Actions(DriverKeeper.GetDriver);

            public IKeyActions KeyDown(string theKey)
            {
                actions.KeyDown(theKey);
                return this;
            }
            public IKeyActions KeyDown(BaseElement element, string theKey)
            {
                actions.KeyDown(element.IWebElement,theKey);
                return this;
            }
            public IKeyActions KeyUp(BaseElement element, string theKey)
            {
                actions.KeyUp(element.IWebElement, theKey);
                return this;
            }
            public IKeyActions KeyUp(string theKey)
            {
                actions.KeyUp(theKey);
                return this;
            }
            public IKeyActions SendKeys(BaseElement element, string keysToSend)
            {
                actions.SendKeys(element.IWebElement,keysToSend);
                return this;
            }
            public IKeyActions SendKeys(string keysToSend)
            {
                actions.SendKeys(keysToSend);
                return this;
            }
            public void Perform() => actions.Perform();
            public void Reset() => actions.Reset();
        }
    }
}