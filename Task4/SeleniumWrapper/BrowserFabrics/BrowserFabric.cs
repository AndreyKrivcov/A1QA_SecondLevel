using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using SeleniumWrapper.Browser;

namespace SeleniumWrapper.BrowserFabrics
{
    public static class BrowserFabric
    {

        private static readonly List<Fabric> fabrics = new List<Fabric>
        {
            new ChromeFabric(),
            new FireFoxFabric()
        };

        public static IBrowser GetBrowser(BrowserType type, DriverOptions options = null, string version = "Latest")
        {
            return GetBrowser(type.ToString(), options, version);
        }
            
        public static IBrowser GetBrowser(string browserName, DriverOptions options = null, string version = "Latest")
        {
            int i = fabrics.FindIndex(x=>x.BrowserName ==browserName);
            if(i > -1)
            {
                return fabrics[i].Create(version,options);
            }

            return null;
        }

        public static bool AddFabric(Fabric fabric)
        {
            if(fabrics.Any(x=>x.BrowserName == fabric.BrowserName))
            {
                return false;
            }

            fabrics.Add(fabric);
            return true;
        }
    }
}