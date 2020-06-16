using System;
using System.Linq;
using System.Collections.Generic;

namespace SeleniumWrapper.Utils
{
    public sealed class Localisation<Param, Language> where Param : Enum where Language : Enum
    {
        private readonly Dictionary<Param, Dictionary <Language, string> > collection = 
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
#endregion

        public void Clear()
        {
            collection.Clear();
        }
    }
}