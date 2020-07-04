using System;
using SeleniumWrapper;
using Tests.Pages.Shared;

namespace Tests.Pages
{
    class CarPage : PageBase
    {
        public bool IsTrimAvailible => throw new NotImplementedException(); // Compare is Compare2Trim link availible or not. If not, return and try again
        public TrimsPage Compare2Trim => throw new NotImplementedException();
        protected override string GetHeadder()
        {
            throw new NotImplementedException();
        }
    }
}