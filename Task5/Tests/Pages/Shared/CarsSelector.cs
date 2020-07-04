using System;

using SeleniumWrapper.Elements;

namespace Tests.Pages.Shared
{
    class CarsSelector
    {
        public CarsSelector(Select make, Select model, Select year)
        {
            Make = new SelectWrapper(make);
            Model = new SelectWrapper(model);
            Year = new SelectWrapper(year);
        }

        private static string forbiddenValuePattern = "All";

        public SelectWrapper Make { get; }
        public SelectWrapper Model { get; }
        public SelectWrapper Year { get; }

        public bool IsFieldsValid => throw new NotImplementedException();
    }
}