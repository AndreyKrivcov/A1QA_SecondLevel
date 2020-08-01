using System.Text.Json;
using SeleniumWrapper;
using SeleniumWrapper.Logging;
using SeleniumWrapper.Browser;
using OpenQA.Selenium;
using System;
using SeleniumWrapper.Elements;

namespace Tests
{
    class Page : BaseForm
    {
        public Page(TimeSpan timeout, params Logger[] loggersCollection) : base(null, true, loggersCollection)
        {   
            this.timeout = timeout;
        }

        private readonly string answerLocator = "//pre"; 

        private readonly TimeSpan timeout;

        public HTMLContent SentAuthentificationData(string login, string password)
        {
            new BasicAuthentificationByUrl
            {
                UserName = login,
                Password = password
            }.Confirm();
            
            return HTMLContent.Desirialize(settings.Browser.Window.FindElement<Text>(By.XPath(answerLocator))
                                                                  .WaitForDisplayed<Text>(timeout)
                                                                  .InnerHTML);
        }
    }

    /*
    {
  "authenticated": true, 
  "user": "user"
}
    */
    class HTMLContent
    {
        public bool authenticated {get;set;}
        public string user {get;set;}

        public static HTMLContent Desirialize(string s)
        {
            return JsonSerializer.Deserialize(s,typeof(HTMLContent)) as HTMLContent;
        }
    }

}