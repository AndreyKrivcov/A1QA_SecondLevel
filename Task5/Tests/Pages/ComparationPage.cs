using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Logging;
using SeleniumWrapper.Elements;
using Tests.Pages.Shared;
using SeleniumWrapper.Utils;

namespace Tests.Pages
{
    class ComparationPage : PageBase
    {
        public ComparationPage(TimeSpan timeout, Logger[] loggers,string make, string model, int year) : base(timeout,loggers)
        {
            carSelector = new CarSelectPopUp(settings.Browser,timeout);
            carSelector.SelectedCar += AddNewDetales; 
            AddNewDetales(make,model,year);
        }
        ~ComparationPage()
        {
            carSelector.SelectedCar-=AddNewDetales;
        }

        private readonly string headderLocator = "//h1[@id=\"main-headline\"]";
        private readonly string addNewCarLocator = "//a[@id=\"add-from-your-favorite-cars-link\"]";
       // private readonly string carNameLocator = "(//div[@ng-switch-when=\"research-car-mmyt\"])[last()]";
     //   private readonly string engineLocator = "((//div[@class=\"info-column\"])[6]//div)[last()-1]/p";

        protected override string GetHeadder()
        {
            return settings.Browser.Window.FindElement<Text>(By.XPath(headderLocator))
                                          .WaitForDisplayed<Text>(timeout).InnerHTML;
        }

        private readonly CarSelectPopUp carSelector;
        public CarSelectPopUp AddNewCar()
        {
            settings.Browser.Window.FindElement<Link>(By.XPath(addNewCarLocator))
                                    .WaitForDisplayed<Link>(timeout).Click();
            carSelector.WaitForDisplayed();
            return carSelector;
        }

        public Dictionary<string, ModelDetales> ComparationDetales => throw new NotImplementedException();

        private void AddNewDetales(string make, string model, int year)
        {
          /*  string carName = settings.Browser.Window.FindElement<Text>(By.XPath(carNameLocator))
                                                    .WaitForDisplayed<Text>(timeout).InnerHTML;
            ModelDetales carDetales = new ModelDetales
            {
                Make = make,
                Model = model,
                Year = year,
                Engine = settings.Browser.Window.FindElement<Text>(By.XPath(engineLocator))
                                                .WaitForDisplayed<Text>(timeout).InnerHTML,
            }*/
        }

        public class CarSelectPopUp
        {
            public CarSelectPopUp(IBrowser browser, TimeSpan timeout)
            {
                this.timeout = timeout;

                CarSelector = new CarsSelector(
                    browser.Window.FindElement<Select>(By.XPath(makeLocator)),
                    browser.Window.FindElement<Select>(By.XPath(modelLocator)),
                    browser.Window.FindElement<Select>(By.XPath(yearLocator)),
                    selectorsForbiddenValues
                );

                doneBtn = browser.Window.FindElement<Button>(By.XPath(doneBtnLocator));
            }

            private readonly TimeSpan timeout;
            
            private readonly string makeLocator = "//select[@id=\"make-dropdown\"]";
            private readonly string modelLocator = "//select[@id=\"model-dropdown\"]";
            private readonly string yearLocator = "//select[@id=\"year-dropdown\"]";
            private readonly string doneBtnLocator = "//button[@class=\"modal-button\"]";

            private readonly ForbiddenValues selectorsForbiddenValues = new ForbiddenValues
            {
                Make = "Make",
                Model = "Model",
                Year = "Year"
            };

            private readonly Button doneBtn;

            public CarsSelector CarSelector { get; }
            
            public void Done()
            {
                string make = CarSelector.Make.Selected;
                string model = CarSelector.Model.Selected;
                int year = Convert.ToInt32(CarSelector.Year.Selected);
                doneBtn.Click();
                SelectedCar?.Invoke(make,model,year);
            }

            public void WaitForDisplayed()
            {
                BrowserWait.Wait(timeout,b=>
                {
                    return CarSelector.Displayed && doneBtn.Displayed;
                },null,typeof(NoSuchElementException));
            }

            public event Action<string, string, int> SelectedCar;
        }
    }
}