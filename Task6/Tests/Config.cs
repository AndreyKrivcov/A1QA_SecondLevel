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
        [XmlElement("PathToFileWithExpectedResults")]
        public string ExpectedAnswersAndTitles {get;set;} = "ExpectedResults.xml";
        [XmlElement("BrowserType")]
        public BrowserType Browser { get; set;} = BrowserType.Chrome;
        [XmlElement("UrlOfMainPage")]
        public string MainUrl { get; set;} = "http://the-internet.herokuapp.com/javascript_alerts";
        [XmlElement("WaitForSeconds")]
        public uint TimeautSeconds { get; set;} = 60;        
    }

    public class ExpectedResults : Serializer<ExpectedResults>
    {
        public string AlertTitle { get; set; } = "I am a JS Alert";
        public string AlertAnswer {get;set;} = "You successfuly clicked an alert";
        public string ConfirmTitle {get;set;} = "I am a JS Confirm";
        public string ConfirmPositiveAnswer {get;set;} = "You clicked: Ok";
        public string PromptTitle {get;set;} = "I am a JS prompt";
        public string PromptAnswer(string myText ) => $"You entered: {myText}";
    }
}