using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using SeleniumWrapper.BrowserFabrics;

namespace SeleniumWrapper
{
    class BrowserFabric
    {

        private readonly List<Fabric> fabrics = new List<Fabric>
        {
            new ChromeFabric(),
            new FireFoxFabric()
        };

        public IBrowser GetBrowser(BrowserType type, string version = "Latest")
        {
            int i = fabrics.FindIndex(x=>x.Type == type);
            if(i > -1)
            {
                return fabrics[i].Create(version);
            }

            return null;
        }
    }
    
    enum BrowserType
    {
        Chrome,
        FireFox
    }
}