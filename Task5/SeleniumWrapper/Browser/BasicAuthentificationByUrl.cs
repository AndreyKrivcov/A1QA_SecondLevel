using System.Linq;

namespace SeleniumWrapper.Browser
{
    public class BasicAuthentificationByUrl
    {
        public string UserName {get;set;}
        public string Password {get;set;}

        public void Confirm()
        {
            string[] data = DriverKeeper.GetDriver.Url.Split(":".ToCharArray());
            if(data.Length != 2)
                throw new System.ArgumentException();
            string newRequest = $"{data[0]}://{UserName}:{Password}@{new string(data[1].Skip(2).ToArray())}";
            DriverKeeper.GetDriver.Url = newRequest;
        }
    }
}