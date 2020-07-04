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
        public MainMenue MainMenue { get; } = new MainMenue();
        protected abstract string GetHeadder();
    }
}