using System;
using SeleniumWrapper;
using Tests.Pages.Shared;

namespace Tests.Pages
{
    class TrimsPage : PageBase
    {
        public TrimsPage(int year, string make, string model)
        {
            Year = year;
            Make = make;
            Model = model;
        }
        public int Year { get; }
        public string Make { get; }
        public string Model { get; }
        public string Engine => throw new NotImplementedException();
        public string Transmission => throw new NotImplementedException();
        protected override string GetHeadder()
        {
            throw new NotImplementedException();
        }

        public static explicit operator ModelDetales(TrimsPage page)
        {
            return new ModelDetales
            {
                Engine = page.Engine,
                Transmission = page.Transmission
            };
        } 
    }
}