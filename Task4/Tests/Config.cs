using SeleniumWrapper.BrowserFabrics;
using Tests.Pages;

namespace Tests
{
    class Config
    {
        public string LogFileName { get; } = "MyLogs.csv";
        public BrowserType browser { get; } = BrowserType.Chrome;
        public string MainUrl { get; } = "https://store.steampowered.com/";
        public Language Language { get; } = Language.En;
        public uint TimeautSeconds { get; } = 60;
        public string PathToDownload { get; } = "/home/andrey/Загрузки/";
    }
}