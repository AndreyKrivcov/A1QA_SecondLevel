using System;
using SeleniumWrapper;

namespace Tests.Pages.Shared
{
    abstract class PageBase : BaseForm
    {
        protected PageBase()
        {
            Headder = GetHeadder();
        }
        public string Headder { get; }
        public Menue MainMenue { get; } = new Menue();
        protected abstract string GetHeadder();

        public class Menue
        {
            public ResearchPage Research => throw new NotImplementedException();
            public HomePage Home => throw new NotImplementedException();
        }
    }
}