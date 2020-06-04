using System;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

using NUnit.Framework;
using SeleniumWrapper;
using SeleniumWrapper.BrowserFabrics;

using Task_3.Pages;
using System.Collections.Generic;

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
                
#region  Asserm Main page
                MainPage mainPage = new MainPage(browser,config.Url,config.WaitSecondsTimeuot,config.WaitMilisecondsSleepage);
                Regex regex = new Regex(@"^Яндекс.Маркет");
                Assert.True(regex.IsMatch(mainPage.Title));
#endregion

#region Addert login 
                Assert.True(mainPage.SignIn.LogIn(config.Login, config.Passward));
#endregion


#region Headder to Name
                var popularGoods = mainPage.PopularGoods;

                Random rnd = new Random();
                int index = rnd.Next(popularGoods.Count);
                string randomCategory_headder = popularGoods[index].Value.Create().Headder;
                string randomCategory_name = popularGoods[index].Key;

                Assert.AreEqual(GoodsCatalog.PopularCategoriesToItsHeadders[randomCategory_name],randomCategory_headder);
#endregion


#region All categoris to it`s popular assets
                browser.Window.Back();

                var allCategories = mainPage.AllCategories;
                File.WriteAllLines(config.PathToFileWithCatigories ,allCategories);

                foreach (var item in popularGoods)
                {
                    Assert.True(GoodsCatalog.AllCategoriesToPopularCategories.Values.Contains(item.Key));
                }

                Assert.True(mainPage.LogOut());
#endregion
            }
        }
    }

    class GoodsCatalog
    {
        public static Dictionary<string, string> AllCategoriesToPopularCategories = new Dictionary<string, string> 
        {
            {"Продукты","Продукты"},
            {"Здоровье","Здоровье"},
            {"Детям","Детям"},
            {"Дом","Дом"},
            {"Красота","Красота"},
            {"Зоотовары",null},
            {"Авто","Авто"},
            {"Спорт и отдых","Спорт"},
            {"Электроника","Электроника"},
            {"Бытовая техника","Бытовая техника"},
            {"Компьютерная техника","Компьютеры"},
            {"Строительство и ремонт","Ремонт"},
            {"Доча, сад и огород","Дача"},
            {"Досуг и развлечения",null},
            {"Оборудование",null},
            {"Одежда и обувь",null}
        };
        public static Dictionary<string, string> PopularCategoriesToItsHeadders = new Dictionary<string, string>
        {
            {"Продукты","Продукты"},
            {"Здоровье","Здоровье"},
            {"Детям","Детские товары"},
            {"Дом","Товары для дома"},
            {"Красота","Товары для красоты"},
            {"Авто","Товары для авто- и мототехники"},
            {"Спорт","Спорт и отдых"},
            {"Электроника","Электроника"},
            {"Бытовая техника","Бытовая техника"},
            {"Компьютеры","Компьютерная техника"},
            {"Ремонт","Строительство и ремонт"},
            {"Дача","Дача, сад и огород"}
        };
        
    }
}
