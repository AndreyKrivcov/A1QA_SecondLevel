using System;
using SeleniumWrapper;
using SeleniumWrapper.Logging;
using Tests.Pages.Shared;

namespace Tests.Pages
{
    class SideBySidePage : PageBase
    {
        public SideBySidePage(TimeSpan timeout, Logger[] loggers) : base(timeout,loggers)
        {
        }
        public CarsSelector CarsSelector => throw new NotImplementedException();
        public ComparationPage StartComparing()
        {
            throw new NotImplementedException();
        }
        protected override string GetHeadder()
        {
            throw new NotImplementedException();
        }
    }
}