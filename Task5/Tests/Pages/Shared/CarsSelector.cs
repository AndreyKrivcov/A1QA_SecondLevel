using System;

using SeleniumWrapper.Elements;

namespace Tests.Pages.Shared
{
    class CarsSelector
    {
        public CarsSelector(Select make, Select model, Select year, ForbiddenValues forbiddenValues)
        {
            Make = new SelectWrapper(make);
            Model = new SelectWrapper(model);
            Year = new SelectWrapper(year);

            this.forbiddenValues = forbiddenValues;
        }

        private readonly ForbiddenValues forbiddenValues;

        public SelectWrapper Make { get; }
        public SelectWrapper Model { get; }
        public SelectWrapper Year { get; }

        public bool IsFieldsValid
        {
            get
            {
                return Make.Selected.ToLower() != forbiddenValues.Make.ToLower() &&
                       Model.Selected.ToLower() != forbiddenValues.Model.ToLower() &&
                       Year.Selected.ToLower() != forbiddenValues.Year.ToLower();
            }
        }

        public bool Displayed => Make.Displayed && Model.Displayed && Year.Displayed;
    }

    struct ForbiddenValues
    {
        public string Make;
        public string Model;
        public string Year;
    }
}