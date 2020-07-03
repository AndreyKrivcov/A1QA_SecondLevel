using System;
using System.Xml.Serialization;
using System.IO;

using SeleniumWrapper.BrowserFabrics;

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
        [XmlElement("WaitForSeconds")]
        public uint TimeautSeconds { get; set;} = 60;
        [XmlElement("DownloadingDirectory")]
        public string PathToDownload { get; set;} = "/home/andrey/Загрузки/";
        [XmlElement("DayOfBirth")]
        public int Day { get; set;} = 31;
        [XmlElement("YearOfBirth")]
        public int Year { get; set;} = 1991; 
        [XmlElement("PathToLocatlisation__Test_1")]
        public string PathToLocalisationForTest_1 {get;set;} = "../../../../Test_1_Localisation.txt";
        [XmlElement("PathToLocatlisation__Test_2")]
        public string PathToLocalisationForTest_2 {get;set;}="../../../../Test_2_Localisation.txt";
        [XmlElement("PathToLocatlisation__MonthNames")]
        public string PathToMonthLocalisation {get;set;}="../../../../MonthNames_Localisation.txt";
        [XmlElement("PathToLocatlisation__LanguageNames")]
        public string PathToLanguageNames {get;set;}="../../../../LanguageNames_Localisation.txt";

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