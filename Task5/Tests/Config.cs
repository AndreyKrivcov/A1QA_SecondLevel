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
        public string MainUrl { get; set;} = "https://www.cars.com/";
        [XmlElement("WaitForSeconds")]
        public uint TimeautSeconds { get; set;} = 60;

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