using SeleniumWrapper.BrowserFabrics;
using Tests.Pages;

namespace Tests
{
    class Config
    {
        public string LogFileName { get; } = "MyLogs.csv";
        public BrowserType browser { get; } = BrowserType.Chrome;
        public string MainUrl { get; } = "https://store.steampowered.com/";
        public string DownloadUrl { get; } = "https://store.steampowered.com/about/";
        public Language Language { get; } = Language.Ru;
        public uint TimeautSeconds { get; } = 60;
        public string PathToDownload { get; } = "/home/andrey/Загрузки/";
    }
}