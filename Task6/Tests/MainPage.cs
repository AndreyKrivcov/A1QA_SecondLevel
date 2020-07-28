using SeleniumWrapper;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Logging;


namespace Tests
{
    class MainPage : BaseForm
    {
        public MainPage(params Logger[] loggersCollection) : base(null, true, loggersCollection)
        {
        }

        public SimpleAlert SimpleAlert()
        {
            throw new System.NotImplementedException();
        }
        public ConfirmAlert ConfirmAlert()
        {
            throw new System.NotImplementedException();
        }
        public PromtAlert PromtAlert()
        {
            throw new System.NotImplementedException();
        }

        public string Answer => throw new System.NotImplementedException();
    }
}