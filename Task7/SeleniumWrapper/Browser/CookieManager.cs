using System;
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
            public void Add(string name, string value) => Add(new Cookie(name, value));
            public void Add(string name, string value, string path) => Add(new Cookie(name, value, path));
            public void Add(string name, string value, string path, DateTime? expiry) => Add(new Cookie(name, value, path, expiry));
            public void Add(string name, string value, string domain, string path, DateTime? expiry) => Add(new Cookie(name, value, domain, path, expiry));

            public ReadOnlyCollection<Cookie> AsReadonly() =>  cookie.AllCookies;

            public void Clear() => cookie.DeleteAllCookies();

            public void Delete(Cookie cookie) => this.cookie.DeleteCookie(cookie);

            public void Delete(string name) => cookie.DeleteCookieNamed(name);
        }
    }
}