using System;
using OpenQA.Selenium;

namespace SeleniumWrapper.Elements
{
    public sealed class A : BaseElement
    {
        public A(By by, int ind, BaseElement parentElemen) : base(by,ind,parentElemen)
        {
            //CheckTag("a");
        }
        public string AccessKey => Element?.GetAttribute("accesskey");
        public string Coords => Element?.GetAttribute("coords");
        public string Href => Element?.GetAttribute("href");
        public string Hreflang => Element?.GetAttribute("hreflang");
        public string Name => Element.GetAttribute("name");
        public string Rel => Element.GetAttribute("rel");
        public string Rev => Element.GetAttribute("rev");
        public Sharpe Sharpe 
        {
            get
            {
                string elem = Element.GetAttribute("sharpe");
                switch(elem)
                {
                    case "circle" : return Sharpe.Circle;
                    case "default" : return Sharpe.Default;
                    case "poly" : return Sharpe.Poly;
                    case "rect" : return Sharpe.Rect;
                    default : return Sharpe.Default;
                }
            }
        }
        public int TabIndex => Convert.ToInt32(Element.GetAttribute("tabindex"));
        public string Target => Element.GetAttribute("target");
        public string Title => Element.GetAttribute("title");
        public string Type => Element.GetAttribute("type");
    }
}