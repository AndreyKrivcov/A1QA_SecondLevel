using System;

namespace Tests.Pages.Shared

{
    class ModelDetales
    {
        public string Engine { get; set; }
        public string Transmission { get; set; }
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
                return x.Engine == y.Engine && x.Transmission == y.Transmission;
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
    }
}