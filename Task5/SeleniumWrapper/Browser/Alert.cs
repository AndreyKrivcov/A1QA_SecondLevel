using OpenQA.Selenium;

namespace SeleniumWrapper.Browser
{
    public class SimpleAlert
    {
        public SimpleAlert()
        {
            alert = DriverKeeper.GetDriver.SwitchTo().Alert();
        }
        protected readonly IAlert alert;
        public void Confirm()
        {
            alert.Accept();
        }
        public string Text => alert.Text;
    }

    public class ConfirmAlert : SimpleAlert
    {
        public void Dismiss()
        {
            alert.Dismiss();
        }
    }

    public class PromtAlert : ConfirmAlert
    {
        public string Message
        {
            set
            {
                alert.SendKeys(value);
            }
        }
    }
}