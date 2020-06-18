using SeleniumWrapper.Browser;
using SeleniumWrapper.BrowserFabrics;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;

namespace Tests
{
    static class BrowserCreator
    {
        private static DriverOptions GetOptions(BrowserType type)
        {
            DriverOptions ans;
            switch(type)
            {
                case BrowserType.Chrome : 
                {
                    ChromeOptions options = new ChromeOptions();
                    options.AddUserProfilePreference("safebrowsing.enabled","false");
                    ans = options;
                }
                break;
                case BrowserType.FireFox :
                {
                    FirefoxOptions option = new FirefoxOptions();
                    option.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/octet-stream");
                    ans = option;
                }
                break;
                default : ans = null; break;
            }

            return ans;
        }

        public static IBrowser GetConfiguredBrowser(BrowserType type)
        {
            return BrowserFabric.GetBrowser(type,GetOptions(type));
        }

    }
}