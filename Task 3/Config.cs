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
    }

    class ConfigSerializer
    {
        private ConfigSerializer(string fileName)
        {
            ConfigFileName = fileName;
        }
        private static DataContractJsonSerializer serializer = 
            new DataContractJsonSerializer(typeof(Config));
        
        private static ConfigSerializer instance;
        public static ConfigSerializer Instance(string fileName = "Test configuration.json")
        {
            if(instance == null)
            {
                instance = new ConfigSerializer(fileName);
            }

            return instance;
        }

        public string ConfigFileName { get; }

        public void Serialize(Config config)
        {
            using(var file = new FileStream(ConfigFileName,FileMode.Create))
            {
                serializer.WriteObject(file,config);
            }
        }
        public Config Deserialize()
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