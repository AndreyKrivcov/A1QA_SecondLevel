using SeleniumWrapper.Browsers;

namespace SeleniumWrapper
{
    class BrowserFabric
    {
        public static IBrowser GetBrowser(BrowserType type, string version = "Latest")
        {
            switch(type)
            {
                case BrowserType.Chrome : return Chrome.Instance(version); 
                case BrowserType.FireFox : return FireFox.Instance(version);
                default : throw new System.ArgumentException("Unsuporting browser");
            }
        }
    }
    
    enum BrowserType
    {
        Chrome,
        FireFox
    }
}