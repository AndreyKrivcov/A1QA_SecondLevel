using SeleniumWrapper.Browser;

namespace SeleniumWrapper
{
    public abstract class BaseForm
    {
        public BaseForm(IBrowser browser)
        {
            this.browser = browser;
        }

        protected readonly IBrowser browser;


    }
}