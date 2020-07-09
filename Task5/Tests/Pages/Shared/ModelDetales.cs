using System;
using System.Linq;

namespace Tests.Pages.Shared

{
    class ModelDetales
    {
        public string[] Engine { get; set; }
        public string[] Transmission { get; set; }

        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public static bool operator ==(ModelDetales x, ModelDetales y)
        {
            if (x is null && y is null) 
            {
                return true;
            }
            else if (x is null || x is null) 
            {
                return false;
            }
            else 
            {
                return x.Engine.IsAnyRepeated(y.Engine) && x.Transmission.IsAnyRepeated(y.Transmission);
            }
        }
        public static bool operator !=(ModelDetales x, ModelDetales y) => !(x == y);

        public override bool Equals(object obj)
        {
            if(obj is ModelDetales detales)
            {
                return this == detales;
            }
            return base.Equals(obj);
        }
        
        public override int GetHashCode() => base.GetHashCode();

        public override string ToString()
        {
            return $"Car: \"{Year} {Make} {Model}\" | Engine: \"{string.Join(',',Engine)}\", Transmission: \"{string.Join(',',Transmission)}\"";
        }
    }

    static class StringArrayExtention
    {
        public static bool IsAnyRepeated(this string[] current, string[] other)
        {
            if(current.Length == 0 || other.Length == 0)
            {
                return false;
            }

            bool ans = false;
            foreach (var item in current)
            {
                if(other.Contains(item))
                {
                    ans = true;
                    break;
                }
            }

            return ans;
        }
    }
}