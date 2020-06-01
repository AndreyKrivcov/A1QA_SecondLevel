using SeleniumWrapper.BrowserFabrics;

namespace Task_3
{
    class Config
    {
        public BrowserType Browser { get; set; }
        public string BrowserVersion { get; set; }
        public string Url { get; set; }
        public string Login { get; set; }
        public string Passward { get; set; }
    }
}