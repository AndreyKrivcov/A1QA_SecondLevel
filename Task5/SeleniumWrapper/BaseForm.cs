using System;
using SeleniumWrapper.Browser;
using SeleniumWrapper.Logging;

using LogType = SeleniumWrapper.Logging.LogType;

namespace SeleniumWrapper
{

    public class InputParams
    {
        public IBrowser Browser = SeleniumWrapper.Browser.Browser.Instance();
        public bool ThreadSaveLogs = false;
        public bool DisableStandartLogging = false;
    }

    public abstract class BaseForm
    {
        protected BaseForm(InputParams settings = null,
                           bool addDefaultConsoleLogger = true, 
                           params Logger[] loggersCollection)
        {
            if(settings != null)
            {
                this.settings = settings;
            }
            browserClosedTougle = !this.settings.Browser.IsOpened;
            logedBrowserClosed = browserClosedTougle;

            Handle = this.settings.Browser.Window.Handle;

            this.settings.Browser.WindowChanged += WindowChanged;
            this.settings.Browser.BrowserClosed += BrowserClosed;
            this.settings.Browser.WindowClosed += WindowClosed;

            if(loggersCollection != null && loggersCollection.Length > 0)
            {
                loggers.Add(loggersCollection);
            }
            if(addDefaultConsoleLogger)
            {
                loggers.Add(LoggerCreator.GetLogger(LoggerTypes.ConsoleLogger,null));
            }

            Log(SeleniumWrapper.Logging.LogType.Info,$"Opened page \"{this.settings.Browser.Window.Url}\"",null);
        }

        ~BaseForm()
        {
            Unsubscribe();
        }

        protected readonly InputParams settings = new InputParams();
        protected readonly LoggersCollection loggers = new LoggersCollection();

#region Properties
        public string Handle { get; }
        public bool WasClosed => browserClosedTougle || windowClosedTougle;
        public string Title
        {
            get
            {
                CheckWindow();
                return settings.Browser.Window.Title;
            }
        }
#endregion

#region Callbacks

#region  WindowChanged tougle
        private bool windowChangedTougle = false;
        private void WindowChanged(string newHandle)
        {
            bool wasChanged = newHandle != Handle;
            if(!settings.DisableStandartLogging && wasChanged && !windowChangedTougle)
            {
                Log(LogType.Info, $"Window with handle \"{Handle}\" switched to window with Title address ({settings.Browser.Window.Title})", 
                    System.Reflection.MethodBase.GetCurrentMethod().Name);            
            }
            else if(!settings.DisableStandartLogging && !wasChanged && windowChangedTougle)
            {
                Log(LogType.Info, $"Window with handle \"{Handle}\" switched back",
                    System.Reflection.MethodBase.GetCurrentMethod().Name);                
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
            if(!settings.DisableStandartLogging && !logedBrowserClosed)
            {
                Log(LogType.Info, "Browser closed", 
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
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
            
            if(!settings.DisableStandartLogging)
            {
                Log(LogType.Info, $"Window with handle \"{Handle}\" was closed",
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            Unsubscribe();
        }
#endregion

        private void Unsubscribe()
        {
            settings.Browser.WindowChanged -= WindowChanged;
            settings.Browser.BrowserClosed -= BrowserClosed;
            settings.Browser.WindowClosed -= WindowClosed;
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
                settings.Browser.SwitchToWindow(Handle);
            }
        }

#region Logger
        protected void Log(LogType type, string msg, string testName, int? testStep = null)
        {
            if(settings.ThreadSaveLogs)
            {
                loggers.ThreadSaveLog(type, msg, testName, testStep);
            }
            else
            {
                loggers.Log(type, msg,testName,testStep);
            }
        }
        protected void Log(Exception e, string testName, int? testStep)
        {
            if(settings.ThreadSaveLogs)
            {
                loggers.ThreadSaveLog(e, testName, testStep);
            }
            else
            {
                loggers.Log(e,testName,testStep);
            }
        }
#endregion

    }
}