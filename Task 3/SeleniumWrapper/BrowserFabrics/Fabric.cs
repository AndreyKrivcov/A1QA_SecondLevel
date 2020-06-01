namespace SeleniumWrapper.BrowserFabrics
{
    abstract class Fabric
    {
        protected Fabric(BrowserType type)
        {
            Type = type;
        }
        
        public abstract IBrowser Create(string version);
        public BrowserType Type{ get; }

    }
}