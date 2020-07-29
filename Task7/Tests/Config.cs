using System;
using System.Collections.Generic;
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
        public string CookieDataFile {get;set;} = "CookiData.xml";
        [XmlElement("BrowserType")]
        public BrowserType Browser { get; set;} = BrowserType.FireFox;
        [XmlElement("UrlOfMainPage")]
        public string MainUrl { get; set;} = "http://example.com";
    }

    public class CookieData : Serializer<CookieData>
    {
        public KeyValuePair First {get;set;} = new  KeyValuePair
        {
            Key = "example_key_1",
            Value = "example_value_1"
        };
        public KeyValuePair Second {get;set;} = new  KeyValuePair
        {
            Key = "example_key_2",
            Value = "example_value_2"
        };
        public KeyValuePair Third {get;set;} = new  KeyValuePair
        {
            Key = "example_key_3",
            Value = "example_value_3"
        };

        public string NewCookie3Value {get;set;} = "example_value_300";

        [Serializable]
        [XmlType(TypeName="CookieKeyValuePair")]
        public struct KeyValuePair
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
    }
}