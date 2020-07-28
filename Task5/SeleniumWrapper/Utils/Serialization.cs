using System.IO;
using System.Xml.Serialization;

namespace SeleniumWrapper.Utils
{
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