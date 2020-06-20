using System;
using System.Xml.Serialization;
using System.IO;

using SeleniumWrapper.BrowserFabrics;
using Tests.Pages;

namespace Tests
{
    [XmlRoot("Settings")]
    public class Config
    {
        [XmlElement("LogFilePath")]
        public string LogFileName { get; set;} = "MyLogs.csv";
        [XmlElement("BrowserType")]
        public BrowserType browser { get; set;} = BrowserType.Chrome;
        [XmlElement("UrlOfMainPage")]
        public string MainUrl { get; set;} = "https://store.steampowered.com/";
        [XmlElement("UrlOfDownloadingPage")]
        public string DownloadUrl { get; set;} = "https://store.steampowered.com/about/";
        [XmlElement]
        public Language Language { get; set;} = Language.En;
        [XmlElement("WaitForSeconds")]
        public uint TimeautSeconds { get; set;} = 60;
        [XmlElement("DownloadingDirectory")]
        public string PathToDownload { get; set;} = "/home/andrey/Загрузки/";
        [XmlElement("DayOfBirth")]
        public int Day { get; set;} = 31;
        [XmlElement("MonthOfBirth")]
        public Month Month {get; set;} = Month.October;
        [XmlElement("YearOfBirth")]
        public int Year { get; set;} = 1991; 

        [NonSerialized]
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(Config));
        public void Serialization(string fileName = "TestConfigurationFile.txt")
        {
            using(FileStream fs = File.Open(fileName,FileMode.Create))
            {
                serializer.Serialize(fs,this);
            }
        }
        public static Config Deserialization(string fileName = "TestConfigurationFile.txt")
        {
            using(FileStream fs = File.Open(fileName, FileMode.Open))
            {
                return serializer.Deserialize(fs) as Config;
            }
        }
    }    
}