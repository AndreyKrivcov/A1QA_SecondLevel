using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using SeleniumWrapper.Browser;

namespace SeleniumWrapper.Elements
{
    internal sealed class DefaultElement<T> : BaseElement where T : BaseElement
    {
        public DefaultElement(WebElementKeeper elementKeeper) : base(elementKeeper)
        {
        }

        public static implicit operator T(DefaultElement<T> element)
        {
            return (T)Activator.CreateInstance(typeof(T),element.element);
        }
    }

    internal static class ElementCollectionExtention
    {
        public static List<T> ToElementArray<T>(this IEnumerable<DefaultElement<T>> collection) where T : BaseElement
        {
            List<T> ans = new List<T>();
            foreach (var item in collection)
            {
                ans.Add(item);
            }

            return ans;
        }

        public static List<T> ToElementArray<T>(this IEnumerable<IWebElement> collection, Func<int,T> outputElementCreator)
        {
            List<T> ans = new List<T>();
            for (int i = 0; i < collection.Count(); i++)
            {
                ans.Add(outputElementCreator(i));
            }

            return ans;
        }
    }
}