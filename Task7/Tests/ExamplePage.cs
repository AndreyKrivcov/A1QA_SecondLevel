using SeleniumWrapper;
using SeleniumWrapper.Browser;

namespace Tests
{
    class ExamplePage : BaseForm
    {
        public ExamplePage(params SeleniumWrapper.Logging.Logger[] loggersCollection) : base(null,true,loggersCollection)
        {
        }

        public ICookieManager Cookies => settings.Browser.Cookie;
    }
}