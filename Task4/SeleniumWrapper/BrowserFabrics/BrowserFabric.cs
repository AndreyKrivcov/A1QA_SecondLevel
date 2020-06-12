using System.Collections.Generic;

namespace SeleniumWrapper.BrowserFabrics
{
    public class BrowserFabric
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
    
    public enum BrowserType
    {
        Chrome,
        FireFox
    }
}