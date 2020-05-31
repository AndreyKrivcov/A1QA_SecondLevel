using WebDriverManager.DriverConfigs.Impl;
using OpenQA.Selenium.Chrome;

namespace SeleniumWrapper.Browsers
{
    class Chrome : BrowserBase
    {
        private Chrome(string version) : base(new ChromeConfig(), version, "Chrome", ()=>new ChromeDriver())
        {}

        private static Chrome instance;

        public static IBrowser Instance(string version)
        {
            if (instance == null)
            {
                instance = new Chrome(version);
            }
            return instance;
        }
    }
}