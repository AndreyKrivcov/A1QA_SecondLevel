using System;
using System.Xml.Serialization;
using System.IO;

using SeleniumWrapper.BrowserFabrics;

namespace Tests
{
    [XmlRoot("Settings")]
    public class Config : Serializer<Config>
    {
        [XmlElement("LogFilePath")]
        public string LogFileName { get; set;} = "MyLogs.csv";
        [XmlElement("HeaddersFilePath")]
        public string PathToHeaddersFile { get; set; } = "ExpectedHeadders.xml";
        [XmlElement("BrowserType")]
        public BrowserType browser { get; set;} = BrowserType.Chrome;
        [XmlElement("UrlOfMainPage")]
        public string MainUrl { get; set;} = "https://www.cars.com/";
        [XmlElement("WaitForSeconds")]
        public uint TimeautSeconds { get; set;} = 60;        
    }    

    [XmlRoot("Headders")]
    public class ExpectedHeadders : Serializer<ExpectedHeadders>
    {
        public string CarPage(int year, string make, string model)
        {
            return $"{year} {make} {model}";
        }  
        [XmlElement("ModelComparationPage")]
        public string ComparationPage { get; set; } = "Model Compare";
        [XmlElement("HomePage")]
        public string HomePage { get; set; } = "Find your next match";
        [XmlElement("ResearchPage")]
        public string ResearchPage { get; set; } = "Make Smart Choices";
        [XmlElement("SideBySidePage")]
        public string SideBySide { get; set; } = " Compare Cars Side-by-Side ";
        public string TrimsPage(string carPageHeadder)
        {
            return $"Compare Trims on the {carPageHeadder}";
        }
    }

    public abstract class Serializer<T> where T : Serializer<T>, new() 
    {
        public void Serialization(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using(FileStream fs = File.Open(fileName,FileMode.Create))
            {
                serializer.Serialize(fs,this);
            }
        }
        public static T Deserialization(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using(FileStream fs = File.Open(fileName, FileMode.Open))
            {
                object deserialized = serializer.Deserialize(fs);
                if(deserialized is T ans)
                {
                    return ans;
                }
                return default(T);
            }
        }

        public static T InstanceOrDeserialize(string fileName) 
        {
            if(File.Exists(fileName))
            {   
                return Deserialization(fileName);
            }
            else
            {
                T ans = new T();
                ans.Serialization(fileName);
                return ans;
            }
        }
    }
}