using System;
using System.Collections.Generic;

using SeleniumWrapper;
using Tests.Pages.Shared;

namespace Tests.Pages
{
    class ComparationPage : PageBase
    {
        public ComparationPage()
        {
            carSelector = new CarSelectPopUp();
            carSelector.SelectedCar += AddNewDetales; 
        }
        ~ComparationPage()
        {
            carSelector.SelectedCar-=AddNewDetales;
        }
        protected override string GetHeadder()
        {
            throw new NotImplementedException();
        }

        private readonly CarSelectPopUp carSelector;
        public CarSelectPopUp AddNewCar()
        {
            throw new NotImplementedException();

            return carSelector;
        }

        public Dictionary<string, ModelDetales> ComparationDetales => throw new NotImplementedException();

        private void AddNewDetales(string selectedCar)
        {
            throw new NotImplementedException();
        }

        public class CarSelectPopUp
        {
            public CarsSelector CarSelector => throw new NotImplementedException();
            public void WaitForOpen() => throw new NotImplementedException();
            public void Done()
            {
                throw new NotImplementedException();
            }

            public event Action<string> SelectedCar;
        }
    }
}