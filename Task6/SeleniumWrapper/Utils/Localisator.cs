using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace SeleniumWrapper.Utils
{
    public sealed class Localisation<Param, Language> where Param : Enum where Language : Enum
    {
        private Dictionary<Param, Dictionary <Language, string> > collection = 
                        new Dictionary<Param, Dictionary<Language, string>>();

#region  Getters
        public string Get(Param param, Language language)
        {
            if(!collection.Keys.Contains(param))
            {
                throw new ArgumentException($"Collection doesn`t contains param \"{param}\"");
            }
            if(!collection[param].Keys.Contains(language))
            {
                throw new ArgumentException($"Collection doesn`t contains language \"{language}\"");
            }

            return collection[param][language];
        }
        public Dictionary<Language, string> Get(Param param)
        {
            if(!collection.Keys.Contains(param))
            {
                throw new ArgumentException($"Collection doesn`t contains param \"{param}\"");
            }

            return collection[param];
        }

        public Dictionary<Language, string> this[Param key]
        {
            get => collection[key];
            set => collection[key] = value;
        }
#endregion

#region Setters
        public void AddOrReplace(Param param, Language language, string value)
        {
            if(collection.Keys.Contains(param))
            {
                if(collection[param].Keys.Contains(language))
                {
                    collection[param][language] = value;
                }
                else
                {
                    collection[param].Add(language,value);
                }
            }
            else
            {
                collection.Add(param, new Dictionary<Language, string> {{language, value}});
            }
        }
        public void AddOrReplace(Param param, Dictionary<Language,string> dictionary)
        {
            if(collection.Keys.Contains(param))
            {
                collection[param] = dictionary;
            }
            else
            {
                collection.Add(param,dictionary);
            }
        }
        public static implicit operator Localisation<Param, Language>(Dictionary<Param,Dictionary<Language,string>> item)
        {
            Localisation<Param, Language> localisation = new Localisation<Param, Language>();
            localisation.collection = item;
            return localisation;
        }
#endregion

        public void Clear()
        {
            collection.Clear();
        }

        public void Serialization(string fileName)
        {
            using (var xmlWriter = new XmlTextWriter(fileName, null))
            {
                void WriteAttribute(string attr, string value)
                {
                    xmlWriter.WriteStartAttribute(attr);
                    xmlWriter.WriteString(value);
                    xmlWriter.WriteEndAttribute();
                }

                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.IndentChar = ' ';
                xmlWriter.Indentation = 4;

                xmlWriter.WriteStartDocument();

                xmlWriter.WriteStartElement("Localisation");

                foreach (var item in collection)
                {
                    xmlWriter.WriteStartElement("Item");

                    WriteAttribute("Name",item.Key.ToString());

                    foreach (var element in item.Value)
                    {
                        xmlWriter.WriteStartElement("Param");

                        WriteAttribute("Language",element.Key.ToString());
                        xmlWriter.WriteString(element.Value);

                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            }
        }
        public void Deserialization(string fileName)
        {
            if(!File.Exists(fileName))
            {
                throw new ArgumentException($"Can`t find file \"{fileName}\"");
            }

            XmlDocument document = new XmlDocument();
            
            document.Load(fileName);

            var elements = document["Localisation"].ChildNodes;
            foreach (XmlElement item in elements)
            {
                Param param = (Param)Enum.Parse(typeof(Param), item.GetAttribute("Name"));
                var data = item.ChildNodes;

                collection.Add(param,new Dictionary<Language, string>());

                foreach (XmlElement translateItem in data)
                {
                    Language language = (Language)Enum.Parse(typeof(Language), translateItem.GetAttribute("Language"));
                    string translate = translateItem.InnerText;

                    collection[param].Add(language,translate);
                }
            }
        }
    }
}