using System;
using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Logging;

using Tests.Pages;
using Tests.Pages.Shared;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            config = Serializer<Config>.InstanceOrDeserialize(fileWithSettings);
            headders = Serializer<ExpectedHeadders>.InstanceOrDeserialize(config.PathToHeaddersFile);

            loggers.Add(new [] {LoggerCreator.GetLogger(LoggerTypes.ConsoleLogger, null),
                                LoggerCreator.GetLogger(LoggerTypes.FileLogger,null,config.LogFileName)});

            browser = BrowserCreator.GetConfiguredBrowser(config.browser);
            browser.Window.Maximize();
            browser.Window.Url = config.MainUrl;
           
        }

        [TearDown]
        public void Teardown()
        {
            browser.Dispose();
        }

#region Settings
        IBrowser browser;
        readonly LoggersCollection loggers = new LoggersCollection();
        readonly string fileWithSettings = "TestConfigurationFile.txt";
        readonly Random rnd = new Random();
        private Config config;
        private ExpectedHeadders headders;
#endregion

        [Test]
        public void Test_Cars()
        {
            string method = System.Reflection.MethodBase.GetCurrentMethod().Name;
            loggers.Log(LogType.Info, $"================================ {method} Started ================================", method,null);

            int testStep = 1;
            try
            {

                Dictionary<string, ModelDetales> carsKeeper = new Dictionary<string, ModelDetales>();

                loggers.Log(LogType.Info, "Open home page", method, testStep);
                HomePage home = new HomePage();
                Assert.AreEqual(home.Headder, headders.HomePage);

                loggers.Log(LogType.Info, "Select random cars", method, testStep);
                var item = GetCarDetales(home, null, testStep);
                carsKeeper.Add(item.Key, item.Value);
                item = GetCarDetales(home,item, testStep);
                carsKeeper.Add(item.Key, item.Value);

                loggers.Log(LogType.Info, "Open Research page", method, ++testStep);
                ResearchPage research = home.MainMenue.Research;
                Assert.AreEqual(research.Headder, headders.ResearchPage);

                loggers.Log(LogType.Info, "Open side by side page", method, testStep);
                SideBySidePage sideBySide = research.SideBySide;
                Assert.AreEqual(sideBySide.Headder, headders.SideBySide);

                loggers.Log(LogType.Info, "Select first car ans open conparation page", method, testStep);
                SelectCar(sideBySide.CarsSelector,
                        carsKeeper.ElementAt(0).Value.Make,
                        carsKeeper.ElementAt(0).Value.Model,
                        carsKeeper.ElementAt(0).Value.Year);
                ComparationPage comparationPage = sideBySide.StartComparing();
                Assert.AreEqual(comparationPage.Headder, headders.ComparationPage);
                Assert.True(comparationPage.ComparationDetales.Keys.Contains(carsKeeper.ElementAt(0).Key));

                loggers.Log(LogType.Info, "Add second car", method, testStep);
                ComparationPage.CarSelectPopUp carSelector = comparationPage.AddNewCar();
                carSelector.WaitForOpen();

                SelectCar(carSelector.CarSelector,
                        carsKeeper.ElementAt(1).Value.Make,
                        carsKeeper.ElementAt(1).Value.Model,
                        carsKeeper.ElementAt(1).Value.Year);
                carSelector.Done();
                Assert.True(comparationPage.ComparationDetales.Keys.Contains(carsKeeper.ElementAt(1).Key));

                loggers.Log(LogType.Info, "Compare cars", method, testStep);
                foreach (var expectedCar in carsKeeper)
                {
                    var actualCar = comparationPage.ComparationDetales[expectedCar.Key];
                    Assert.True(expectedCar.Value == actualCar,$"Expected: {expectedCar.Value.ToString()} \n Actual: {actualCar.ToString()}");
                }           
            }
            catch(Exception e)
            {
                loggers.Log(e,method,testStep);
            }

            loggers.Log(LogType.Info, $"================================ {method} Finished ================================",method,null); 
        }

        void SelectCar(CarsSelector selector, string make, string model, int year)
        {
            selector.Make.Selected = make;
            selector.Model.Selected = model;
            selector.Year.Selected = year.ToString();
        }

        KeyValuePair<string,ModelDetales> GetCarDetales(HomePage home, KeyValuePair<string, ModelDetales>? lastSelected, int testStep)
        {
            string method = System.Reflection.MethodBase.GetCurrentMethod().Name;

            loggers.Log(LogType.Info, "Open Research page", method, testStep);
            ResearchPage research = home.MainMenue.Research;
            Assert.AreEqual(research.Headder,headders.ResearchPage);

            string selectCar()
            {
                string ans = SelectRandomCar(research.CarsSelector);
                if(lastSelected.HasValue && ans == lastSelected.Value.Key)
                {
                    loggers.Log(LogType.Info, "Selectted car has already been selected. Useing recursion to select new car", method, testStep);
                    return selectCar();
                }

                return ans;
            }

            loggers.Log(LogType.Info, "Select new car", method, testStep);
            string carPageHeadder = selectCar();
            CarPage carPage = research.Car();
            Assert.AreEqual(carPageHeadder, carPage.Headder);

            if(!carPage.IsTrimAvailible)
            {
                loggers.Log(LogType.Info, "This car don`t has \"Trim\" oprtion. Using recursion to select new car ", method, testStep);
                return GetCarDetales(carPage.MainMenue.Home, lastSelected, testStep);
            }
            
            loggers.Log(LogType.Info, "Open Trim page", method, testStep);
            TrimsPage trim = carPage.Compare2Trim;
            Assert.AreEqual(trim, headders.TrimsPage(carPageHeadder));

            loggers.Log(LogType.Info, "Select cars detales", method, testStep);
            var ans = new KeyValuePair<string, ModelDetales>(carPageHeadder, (ModelDetales)trim);

            loggers.Log(LogType.Info, "Go to the Home page", method, testStep);
            Assert.AreEqual(trim.MainMenue.Home, headders.HomePage);

            return ans;
        }

        string SelectRandomCar(CarsSelector carsSelector)
        {
            SelectRandom(carsSelector.Make);
            SelectRandom(carsSelector.Model);
            SelectRandom(carsSelector.Year);

            if(!carsSelector.IsFieldsValid)
            {
                throw new Exception("Wrong values was selected in cars selectors! Check logic of the method \"void SelectRandom(SelectWrapper selector)\"");
            }

            return headders.CarPage(Convert.ToInt32(carsSelector.Year.Selected), carsSelector.Make.Selected, carsSelector.Model.Selected);
        }
        void SelectRandom(SelectWrapper selector)
        {
            var possibleValues = selector.PossibleValues;
            int i = rnd.Next(1,possibleValues.Count);
            selector.Selected = possibleValues[i];
        }

    }
}
