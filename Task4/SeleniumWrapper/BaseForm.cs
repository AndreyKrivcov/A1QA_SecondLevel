using System;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Elements;
using SeleniumWrapper.Logging;

using LogType = SeleniumWrapper.Logging.LogType;

namespace SeleniumWrapper
{
    public abstract class BaseForm
    {
        protected BaseForm(IBrowser browser, string url = null, 
                            bool openNewWindow = false, 
                            bool threadSaveLogs = false, 
                            bool disableStandartLogging = false)
        {
            this.browser = browser;
            this.threadSaveLogs = threadSaveLogs;
            this.disableStandartLogging = disableStandartLogging;
            browserClosedTougle = false;
            logedBrowserClosed = false;

            if(!string.IsNullOrEmpty(url) && !string.IsNullOrWhiteSpace(url))
            {
                if(openNewWindow)
                {
                    browser.NewWindow(url);
                }
                else
                {
                    browser.Window.Url = url;
                }
            }

            Handle = browser.Window.Handle;
            Url = browser.Window.Url;

            browser.WindowChanged += WindowChanged;
            browser.BrowserClosed += BrowserClosed;
            browser.WindowClosed += WindowClosed;

            loggers.Add(LoggerCreator.GetLogger(LoggerTypes.ConsoleLogger,""));
        }

        ~BaseForm()
        {
            Unsubscribe();
        }

        protected readonly IBrowser browser;
        protected readonly LoggersCollection loggers = new LoggersCollection();
        private readonly bool threadSaveLogs;
        private readonly bool disableStandartLogging;

#region Properties
        public string Handle { get; }
        public bool WasClosed => browserClosedTougle || windowClosedTougle;
        public string Title
        {
            get
            {
                CheckWindow();
                return browser.Window.Title;
            }
        }
        public string Url { get; protected set; }
#endregion

#region Callbacks

#region  WindowChanged tougle
        private bool windowChangedTougle = false;
        private void WindowChanged(string newHandle)
        {
            bool wasChanged = newHandle != Handle;
            if(!disableStandartLogging && wasChanged && !windowChangedTougle)
            {
                Log(LogType.Info, $"Window with URL address ({Url}) switched to window with URL address ({browser.Window.Url})", 
                    System.Reflection.MethodBase.GetCurrentMethod().Name, 0);            
            }
            else if(!disableStandartLogging && !wasChanged && windowChangedTougle)
            {
                Log(LogType.Info, $"Window with URL address ({Url}) switched back",
                    System.Reflection.MethodBase.GetCurrentMethod().Name, 0);                
            }

            windowChangedTougle = wasChanged;
        }
#endregion

#region BrowserClosed tougle
        private static bool browserClosedTougle;
        private static bool logedBrowserClosed;
        private void BrowserClosed()
        {
            if(!browserClosedTougle)
            {
                browserClosedTougle = true;
            }
            if(!disableStandartLogging && !logedBrowserClosed)
            {
                Log(LogType.Info, "Browser closed", 
                    System.Reflection.MethodBase.GetCurrentMethod().Name, 0);
                logedBrowserClosed = true;
            }
            Unsubscribe();
        }
#endregion

#region WindowClose tougle
        private bool windowClosedTougle = false;
        private void WindowClosed(string handle)
        {
            windowClosedTougle = handle == Handle;
            Unsubscribe();
            if(!disableStandartLogging)
            {
                Log(LogType.Info, $"Window with URL address ({Url}) was closed",
                    System.Reflection.MethodBase.GetCurrentMethod().Name, 0);
            }
        }
#endregion

        private void Unsubscribe()
        {
            browser.WindowChanged -= WindowChanged;
            browser.BrowserClosed -= BrowserClosed;
            browser.WindowClosed -= WindowClosed;
        }
#endregion

        protected void CheckWindow()
        {
            if(WasClosed)
            {
                throw new Exception("Widow was closed");
            }

            if(windowChangedTougle)
            {
                browser.SwitchToWindow(Handle);
            }

            string lastUrl = browser.Window.Url;
            if(Url != lastUrl)
            {
                browser.Window.Url = Url;
                if(!disableStandartLogging)
                {
                    Log(LogType.Info, $"Restore URL adress from ({lastUrl}), to URL ({Url})", 
                        System.Reflection.MethodBase.GetCurrentMethod().Name, 0);
                }
            }
        }

#region Logger
        protected void Log(LogType type, string msg, string testName, int testStep)
        {
            if(threadSaveLogs)
            {
                loggers.ThreadSaveLog(type, msg, testName, testStep);
            }
            else
            {
                loggers.TestName = testName;
                loggers.TestStep = testStep;
                loggers.Log(type, msg);
            }
        }
        protected void Log(Exception e, string testName, int testStep)
        {
            if(threadSaveLogs)
            {
                loggers.ThreadSaveLog(e, testName, testStep);
            }
            else
            {
                loggers.TestName = testName;
                loggers.TestStep = testStep;
                loggers.Log(e);
            }
        }
#endregion

#region Wait
        protected T WaitForElement<T>(By by, TimeSpan timeout, TimeSpan? sleepInterval = null, params Type[] ignoringExceptions) where T : BaseElement
        {
            CheckWindow();

            if(ignoringExceptions != null && 
               ignoringExceptions.Contains(typeof(NoSuchElementException)))
            {
                ignoringExceptions = ignoringExceptions.Concat(new [] { typeof(NoSuchElementException)}).ToArray();
            }
            return Wait(timeout, (IBrowser b)=> 
            {
                var element = b.Window.FindElement<T>(by);
                return (element.IsExists ? element : null);
            }, sleepInterval, ignoringExceptions);
        }

        protected ReadOnlyCollection<T> WaitForElements<T>(By by, TimeSpan timeout, TimeSpan? sleepInterval = null, params Type[] ignoringExceptions)
                                    where T : BaseElement
        {
            CheckWindow();
            
            if(ignoringExceptions != null && 
               ignoringExceptions.Contains(typeof(NoSuchElementException)))
            {
                ignoringExceptions = ignoringExceptions.Concat(new [] { typeof(NoSuchElementException)}).ToArray();
            }

            ReadOnlyCollection<T> elements = null;
            Wait(timeout, (IBrowser b)=>
            {
                elements = b.Window.FindElements<T>(by);
                foreach (var item in elements)
                {
                    if(!item.IsExists)
                    {
                        return false;
                    }
                }

                return true;
            },sleepInterval,ignoringExceptions);

            return elements;
        }

        protected T Wait<T>(TimeSpan timeout, Func<IBrowser, T> f, TimeSpan? sleepInterval = null, params Type[] ignoringExceptions)
        {
            var wait = sleepInterval.HasValue ? new BrowserWait(new SystemClock(),browser,timeout, sleepInterval.Value) 
                                              : new BrowserWait(browser,timeout);

            if(ignoringExceptions != null && ignoringExceptions.Count() > 0)
            {
                wait.IgnoreExceptionTypes(ignoringExceptions);
            }

            return wait.Until(x=> f(x));
        }
#endregion

    }
}