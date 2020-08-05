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
        [XmlElement("PathToLoginFile")]
        public string PathToLoginAndPasswordFile {get;set;} = "LoginAndPasswordData.xml";
        [XmlElement("BrowserType")]
        public BrowserType Browser { get; set;} = BrowserType.Chrome;
        [XmlElement("UrlOfMainPage")]
        public string MainUrl { get; set;} = "https://httpbin.org/basic-auth/user/passwd";
        [XmlElement("WaitForSeconds")]
        public uint TimeautSeconds { get; set;} = 60;        
    }

    public class LoginAndPassword : Serializer<LoginAndPassword>
    {
        public string Login {get;set;}    
        public string Password {get;set;}
    }
}