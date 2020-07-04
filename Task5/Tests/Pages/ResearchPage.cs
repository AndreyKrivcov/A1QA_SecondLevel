using System;

using SeleniumWrapper;
using Tests.Pages.Shared;

namespace Tests.Pages
{
    class ResearchPage : PageBase
    {
        public CarsSelector CarsSelector => throw new NotImplementedException();

        public CarPage Car()
        {            
            throw new NotImplementedException();
        }
        public SideBySidePage SideBySide => throw new NotImplementedException();

        protected override string GetHeadder()
        {
            throw new NotImplementedException();
        }
    }
}