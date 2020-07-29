using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace SeleniumWrapper.Browser
{
    internal partial class Browser
    {
        private sealed class CookieManager : ICookieManager
        {
            public CookieManager()
            {
                cookie = DriverKeeper.GetDriver.Manage().Cookies;
            }
            private readonly ICookieJar cookie;
            public Cookie this[string name] => cookie.GetCookieNamed(name);

            public void Add(Cookie cookie) => this.cookie.AddCookie(cookie);

            public ReadOnlyCollection<Cookie> AsReadonly() =>  cookie.AllCookies;

            public void Clear() => cookie.DeleteAllCookies();

            public void Delete(Cookie cookie) => this.cookie.DeleteCookie(cookie);

            public void Delete(string name) => cookie.DeleteCookieNamed(name);
            public IEnumerator<Cookie> GetEnumerator() => cookie.AllCookies.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => cookie.AllCookies.GetEnumerator();
        }
    }
}