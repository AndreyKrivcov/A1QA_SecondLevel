using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumWrapper.Elements;

namespace SeleniumWrapper.Utils
{
    public sealed class MouseUtils : IAction
    {
        public MouseUtils(IWebDriver driver)
        {
            actions = new OpenQA.Selenium.Interactions.Actions(driver);
        }
        private readonly OpenQA.Selenium.Interactions.Actions actions;

        public MouseUtils Click()
        {
            actions.Click();
            return this;
        } 
        public MouseUtils Click(BaseElement onElement) 
        {
            actions.Click(onElement.IWebElement);
            return this;
        }
        public MouseUtils ClickAndHold(BaseElement onElement)
        {
            actions.ClickAndHold(onElement.IWebElement);
            return this;
        }
        public MouseUtils ClickAndHold()
        {
            actions.ClickAndHold();
            return this;
        }
        public MouseUtils ContextClick()
        {
            actions.ContextClick();
            return this;
        }
        public MouseUtils ContextClick(BaseElement onElement)
        {
            actions.ContextClick(onElement.IWebElement);
            return this;
        }
        public MouseUtils DoubleClick()
        {
            actions.DoubleClick();
            return this;
        }
        public MouseUtils DoubleClick(BaseElement onElement)
        {
            actions.DoubleClick(onElement.IWebElement);
            return this;
        }
        public MouseUtils DragAndDrop(BaseElement source, BaseElement target)
        {
            actions.DragAndDrop(source.IWebElement, target.IWebElement);
            return this;
        }
        public MouseUtils DragAndDropToOffset(BaseElement source, int offsetX, int offsetY)
        {
            actions.DragAndDropToOffset(source.IWebElement, offsetX, offsetY);
            return this;
        }
        public MouseUtils MoveByOffset(int offsetX, int offsetY)
        {
            actions.MoveByOffset(offsetX, offsetY);
            return this;
        }
        public MouseUtils MoveToElement(BaseElement toElement)
        {
            actions.MoveToElement(toElement.IWebElement);
            return this;
        }
        public MouseUtils MoveToElement(BaseElement toElement, int offsetX, int offsetY)
        {
            actions.MoveToElement(toElement.IWebElement, offsetX, offsetY);
            return this;
        }
        public MouseUtils MoveToElement(BaseElement toElement, int offsetX, int offsetY, MoveToElementOffsetOrigin offsetOrigin)
        {
            actions.MoveToElement(toElement.IWebElement,offsetX,offsetY,offsetOrigin);
            return this;
        }
        public MouseUtils Release(BaseElement onElement) 
        {
            actions.Release(onElement.IWebElement);
            return this;
        }
        public MouseUtils Release()
        {
            actions.Release();
            return this;
        }
        public void Perform() => actions.Perform();    
        public void Reset() => actions.Reset();
    }
}