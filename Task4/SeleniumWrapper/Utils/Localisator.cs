using System;
using System.Linq;
using System.Collections.Generic;

namespace SeleniumWrapper.Utils
{
    public sealed class Localisator<T> where T  : Enum
    {
        private readonly Dictionary<string, Dictionary <T, string> > collection = 
                        new Dictionary<string, Dictionary<T, string>>();

#region  Getters
        public string Get(string param, T language)
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
        public Dictionary<T, string> Get(string param)
        {
            if(!collection.Keys.Contains(param))
            {
                throw new ArgumentException($"Collection doesn`t contains param \"{param}\"");
            }

            return collection[param];
        }

        public Dictionary<T, string> this[string key]
        {
            get => collection[key];
            set => collection[key] = value;
        }
#endregion

#region Setters
        public void AddOrReplace(string param, T language, string value)
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
                collection.Add(param, new Dictionary<T, string> {{language, value}});
            }
        }
        public void AddOrReplace(string param, Dictionary<T,string> dictionary)
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

        void Clear()
        {
            collection.Clear();
        }
    }
}