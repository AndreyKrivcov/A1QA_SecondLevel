using SeleniumWrapper.Browser;

namespace SeleniumWrapper.Elements
{
    public sealed class Link : BaseElement
    {
        public Link(WebElementKeeper elementKeeper) : base(elementKeeper)
        {
        }        
        public string Href => Element?.GetAttribute("href");
        public string Hreflang => Element?.GetAttribute("hreflang");        
        public string Rel => Element.GetAttribute("rel");
        public string Target => Element.GetAttribute("target");
        
    }
}