using System.Xml.Serialization;
using SeleniumWrapper.BrowserFabrics;
using SeleniumWrapper.Utils;

namespace Tests
{
    [XmlRoot("Settings")]
    public class Config : Serializer<Config>
    {
        [XmlElement("LogFilePath")]
        public string LogFileName { get; set;} = "MyLogs.csv";
        [XmlElement("BrowserType")]
        public BrowserType Browser { get; set;} = BrowserType.Chrome;
        [XmlElement("UrlOfMainPage")]
        public string MainUrl { get; set;} = "http://the-internet.herokuapp.com/iframe";
        public string FileWithExpectedValues {get;set;} = "ExpectedValues.xml";
    }

    public class ExpectedValues : Serializer<ExpectedValues>
    {
        public string Title {get;set;} = "An iFrame containing the TinyMCE WYSIWYG Editor";
    }
}