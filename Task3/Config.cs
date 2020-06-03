using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using SeleniumWrapper.BrowserFabrics;

namespace Task_3
{

    [DataContract]
    class Config
    {
        public BrowserType Browser { get; set; } = BrowserType.Chrome;
        [DataMember]
        private string BrowserTypeName 
        { 
            get => Enum.GetName(typeof(BrowserType), Browser);
            set 
            {
                BrowserType ans;
                if(Enum.TryParse<BrowserType>(value,true, out ans))
                {
                    Browser = ans;
                }
            } 
        }
        [DataMember]
        public string BrowserVersion { get; set; } = "Latest";
        [DataMember]
        public string Url { get; set; } = "https://market.yandex.ru";
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public string Passward { get; set; }
        [DataMember]
        public uint WaitSecondsTimeuot { get; set; } = 60;
        [DataMember]
        public uint WaitMilisecondsSleepage { get; set; } = 500;
    }

    class ConfigSerializer
    {
        private static DataContractJsonSerializer serializer = 
            new DataContractJsonSerializer(typeof(Config));
        public static string ConfigFileName { get; } = "Test configuration.json";

        public static void Serialize(Config config)
        {
            using(var file = new FileStream(ConfigFileName,FileMode.Create))
            {
                serializer.WriteObject(file,config);
            }
        }
        public static Config Deserialize()
        {
            if(!File.Exists(ConfigFileName))
            {
                return null;
            }
            
            using(var file = new FileStream(ConfigFileName,FileMode.Open))
            {
                return (Config)serializer.ReadObject(file);
            }
        }
    }
}