
using System;
using OpenQA.Selenium.Support.UI;
using SeleniumWrapper;

namespace Task_3.Pages
{
    class AuthorizationPage : PageBase
    {
        public AuthorizationPage(IBrowser browser, TimeSpan timeout_s, TimeSpan sleep_mls) : base (browser, browser.Window.Url)
        {
            wait = new BrowserWait(new SystemClock(),browser,timeout_s,sleep_mls);
        }

#region Fluent wait 
        private readonly BrowserWait wait;
#endregion

#region Selevtors

#region Input parametres 
        private readonly string login_xpath = "//div[@data-t=\"field:login\"]//input";
        private readonly string password = "//div[@data-t=\"field:passwd\"]//input";
        private readonly string submitButton = "//button[@type=\"submit\"]";
#endregion

#region Failed data
        private readonly string failedPasswordNotification = "//div[@data-t=\"field:passwd\"]/div[2]";
        private readonly string failedPasswordNotificationString = "Неверный пароль";
#endregion

#endregion

        public bool LogIn(string login, string pass)
        {
            try
            {
                FindElements(login_xpath,wait)[0].SendKeys(login);
                Submit();
                FindElements(password,wait)[0].SendKeys(pass);
                Submit();
                
                if(FindElements(failedPasswordNotification,wait)[0].Text == failedPasswordNotificationString)
                {
                    return false;
                }

                return wait.Until(x=>
                {
                    return (!x.OpenedWindows.Contains(MyHandle));
                });
            }
            catch(Exception)
            {
                return false;
            }
        }

        private void Submit()
        {
            FindElements(submitButton,wait)[0].Click();
        }
    }
}