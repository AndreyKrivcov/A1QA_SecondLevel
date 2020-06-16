using System.Collections.Generic;
using System.Linq;

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

        public static IBrowser GetBrowser(BrowserType type, string version = "Latest") => GetBrowser(type.ToString(), version);
            
        public static IBrowser GetBrowser(string browserName, string version = "Latest")
        {
            int i = fabrics.FindIndex(x=>x.BrowserName ==browserName);
            if(i > -1)
            {
                return fabrics[i].Create(version);
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