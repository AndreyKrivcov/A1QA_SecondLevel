using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumWrapper.Elements;

namespace SeleniumWrapper.Browser
{
    internal partial class Browser
    {
        private sealed class MouseActionsManager : IMouseActions
        {        
            private readonly OpenQA.Selenium.Interactions.Actions actions = 
                new OpenQA.Selenium.Interactions.Actions(DriverKeeper.GetDriver);

            public IMouseActions Click()
            {
                actions.Click();
                return this;
            } 
            public IMouseActions Click(BaseElement onElement) 
            {
                actions.Click(onElement.IWebElement);
                return this;
            }
            public IMouseActions ClickAndHold(BaseElement onElement)
            {
                actions.ClickAndHold(onElement.IWebElement);
                return this;
            }
            public IMouseActions ClickAndHold()
            {
                actions.ClickAndHold();
                return this;
            }
            public IMouseActions ContextClick()
            {
                actions.ContextClick();
                return this;
            }
            public IMouseActions ContextClick(BaseElement onElement)
            {
                actions.ContextClick(onElement.IWebElement);
                return this;
            }
            public IMouseActions DoubleClick()
            {
                actions.DoubleClick();
                return this;
            }
            public IMouseActions DoubleClick(BaseElement onElement)
            {
                actions.DoubleClick(onElement.IWebElement);
                return this;
            }
            public IMouseActions DragAndDrop(BaseElement source, BaseElement target)
            {
                actions.DragAndDrop(source.IWebElement, target.IWebElement);
                return this;
            }
            public IMouseActions DragAndDropToOffset(BaseElement source, int offsetX, int offsetY)
            {
                actions.DragAndDropToOffset(source.IWebElement, offsetX, offsetY);
                return this;
            }
            public IMouseActions MoveByOffset(int offsetX, int offsetY)
            {
                actions.MoveByOffset(offsetX, offsetY);
                return this;
            }
            public IMouseActions MoveToElement(BaseElement toElement)
            {
                actions.MoveToElement(toElement.IWebElement);
                return this;
            }
            public IMouseActions MoveToElement(BaseElement toElement, int offsetX, int offsetY)
            {
                actions.MoveToElement(toElement.IWebElement, offsetX, offsetY);
                return this;
            }
            public IMouseActions MoveToElement(BaseElement toElement, int offsetX, int offsetY, MoveToElementOffsetOrigin offsetOrigin)
            {
                actions.MoveToElement(toElement.IWebElement,offsetX,offsetY,offsetOrigin);
                return this;
            }
            public IMouseActions Release(BaseElement onElement) 
            {
                actions.Release(onElement.IWebElement);
                return this;
            }
            public IMouseActions Release()
            {
                actions.Release();
                return this;
            }
            public void Perform() => actions.Perform();    
            public void Reset() => actions.Reset();
        }
    }
}