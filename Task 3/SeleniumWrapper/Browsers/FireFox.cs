using WebDriverManager.DriverConfigs.Impl;
using OpenQA.Selenium.Firefox;

namespace SeleniumWrapper.Browsers
{
    class FireFox : BrowserBase
    {
        private FireFox(string version) : base(new FirefoxConfig(), version, "FireFox", ()=>new FirefoxDriver() )
        {}

        private static FireFox instance;

        public static IBrowser Instance(string version)
        {
            if (instance == null)
            {
                instance = new FireFox(version);
            }
            return instance;
        }
    }
}