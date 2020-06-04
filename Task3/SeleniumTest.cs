using System;
using System.Linq;
using System.IO;

using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumWrapper;
using SeleniumWrapper.BrowserFabrics;

using Task_3.Pages;

namespace Task_3
{
    public class SeleniumTest
    {
        [SetUp]
        public void Setup()
        {
            config = ConfigSerializer.Deserialize();
            if(config == null)
            {
                config = new Config();
                ConfigSerializer.Serialize(config);
            }
        }

        Config config;
        
        [Test]
        public void Test_YandexMarket()
        {
            using( IBrowser browser = new BrowserFabric().GetBrowser(config.Browser, config.BrowserVersion))
            {
                browser.Window.Maximize();
                
                MainPage mainPage = new MainPage(browser,config.Url,config.WaitSecondsTimeuot,config.WaitMilisecondsSleepage);
                /*Assert по регулярному вырожению Title содержит в себе 'Яндекс.Маркет'*/

                Assert.True(mainPage.SignIn.LogIn(config.Login, config.Passward));
                
                var popularGoods = mainPage.PopularGoods;

                Random rnd = new Random();
                int index = rnd.Next(popularGoods.Count);
                string randomCategory_headder = popularGoods[index].Value.Create().Headder;
                string randomCategory_name = popularGoods[index].Key;

                /*Assert that headder belongs to the name*/

                browser.Window.Back();

                var allCategories = mainPage.AllCategories;
                File.WriteAllLines(config.PathToFileWithCatigories ,allCategories);

                /*Assert that allCategories containes popularGoods*/

                Assert.True(mainPage.LogOut());
            }
        }
    }
}
