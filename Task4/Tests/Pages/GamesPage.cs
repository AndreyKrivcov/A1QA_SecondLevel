using System;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumWrapper;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Elements;

namespace Tests.Pages
{
    class GamesPage
    {
        public GamesPage(AgeVerificationData verificationData)
        {

        }
        public GamesPage()
        {

        }
    }

    struct AgeVerificationData
    {

    }

    class AgeVerificationPage : BaseForm
    {
        public AgeVerificationPage(IBrowser browser, TimeSpan timeout) : base(browser)
        {
            this.timeout = timeout;
            Log(SeleniumWrapper.Logging.LogType.Info,$"Opened page \"{Url}\"","", 0);
        }

        private readonly TimeSpan timeout;

        public bool IsPageOpened
        {
            get
            {
                try
                {
                    bool isOpened = Day.Displayed && Month.Displayed && Year.Displayed && OpenPage.Displayed;
                    if(isOpened)
                    {
                        Log(SeleniumWrapper.Logging.LogType.Info,"Age verification page opened", "DiscountTest", 1);
                    }
                    return isOpened;
                }
                catch(Exception)
                {
                    return false;
                }
            }
        }

#region Locators
        private static readonly string dayLocator = "//select[@id=\"ageDay\"]";
        private static readonly string monthLocator = "//select[@id=\"ageMonth\"]";
        private static readonly string yearLocator = "//select[@id=\"ageYear\"]";
        private static readonly string submitLocator = "//div[@class=\"agegate_text_container btns\"]/a[1]";
#endregion


        private T Getter<T>(string xPath, ref T element) where T : BaseElement
        {
            if(element == null)
            {
                element = WaitForElement<T>(By.XPath(xPath), timeout);
            }

            return element;
        }

        private Select day;
        public Select Day => Getter(dayLocator,ref day);
        private Select month;
        public Select Month => Getter(monthLocator,ref month);
        private Select year;
        public Select Year => Getter(yearLocator,ref year);
        private A submit;
        private A OpenPage => Getter(submitLocator,ref submit);

        public void Submit()=>OpenPage.Click();

    }
}