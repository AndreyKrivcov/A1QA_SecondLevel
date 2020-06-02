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

        private static DataContractJsonSerializer serializer = 
            new DataContractJsonSerializer(typeof(Config));
        public void Serialize(string filePath)
        {
            using(var file = new FileStream(filePath,FileMode.Create))
            {
                serializer.WriteObject(file,this);
            }
        }
        public static Config Deserialize(string filePath)
        {
            if(!File.Exists(filePath))
            {
                return null;
            }
            
            using(var file = new FileStream(filePath,FileMode.Open))
            {
                return (Config)serializer.ReadObject(file);
            }
        }
    }
}