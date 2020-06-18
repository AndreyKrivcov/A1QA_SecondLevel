using OpenQA.Selenium;

namespace SeleniumWrapper.Elements
{
    public sealed class Div : BaseElement
    {
        public Div(By by, int ind, BaseElement parentElemen) : base(by, ind, parentElemen)
        {
        }

        public string Title => Element.GetAttribute("title");
        public Align Align 
        {
            get
            {
                string align = Element.GetAttribute("align");
                switch (align)
                {
                    case "center" : return Align.Center;
                    case "left" : return Align.Left;
                    case "right" : return Align.Right;
                    case "justify" : return Align.Justify;
                    default: return Align.Left;
                }
            }
        }
    }
}