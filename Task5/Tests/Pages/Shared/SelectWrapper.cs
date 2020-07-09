using System.Collections.ObjectModel;
using System.Linq;

using SeleniumWrapper.Elements;

namespace Tests.Pages.Shared
{
    class SelectWrapper
    {
        public SelectWrapper(Select select)
        {
            this.select = select;
        }

        private readonly Select select;

        public string Selected
        {
            get => select.SelectedOption.InnerHTML;
            set => select.SelectByText(value);
        }

        public bool Displayed => select.Disabled;

        public ReadOnlyCollection<string> PossibleValues =>
            select.Options.Select(x=>x.InnerHTML).ToList().AsReadOnly();
    }
}