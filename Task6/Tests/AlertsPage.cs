using SeleniumWrapper;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Logging;
using SeleniumWrapper.Elements;
using OpenQA.Selenium;

namespace Tests
{
    class AlertsPage : BaseForm
    {
        public AlertsPage(params Logger[] loggersCollection) : base(null, true, loggersCollection)
        {
        }

#region Locators
        private readonly string AnswerLocator = "//p[@id=\"result\"]";
        private readonly string AlertBtnLocator = "//button[contains(text(),\"Alert\")]";
        private readonly string ConfirmBtnLocator = "//button[contains(text(),\"Confirm\")]";
        private readonly string PromtBtnLocator = "//button[contains(text(),\"Prompt\")]";
#endregion

        public SimpleAlert SimpleAlert()
        {
            settings.Browser.Window.FindElement<Button>(By.XPath(AlertBtnLocator)).Click();
            return new SimpleAlert();
        }
        public ConfirmAlert ConfirmAlert()
        {
            settings.Browser.Window.FindElement<Button>(By.XPath(ConfirmBtnLocator)).Click();
            return new ConfirmAlert();
        }
        public PromtAlert PromtAlert()
        {
            settings.Browser.Window.FindElement<Button>(By.XPath(PromtBtnLocator)).Click();
            return new PromtAlert();
        }

        public string Answer => settings.Browser.Window.FindElement<Text>(By.XPath(AnswerLocator)).InnerHTML;
    }
}