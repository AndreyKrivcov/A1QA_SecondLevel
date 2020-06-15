using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SeleniumWrapper.Logging
{
    public abstract class Logger
    {
        protected Logger(Func<StreamWriter> streamCreator, string loggerName)
        {
            this.streamCreator = streamCreator;
            LoggerName = loggerName;
        }

        protected readonly Func<StreamWriter> streamCreator;

        public virtual string TestName { get; set; }
        public virtual int TestStep { get; set; }
        public virtual string OutputPath{ get; protected set;}
        public string LoggerName { get; }

        public virtual void Log(LogType type, string msg)
        {
            using(StreamWriter sw = streamCreator())
            {
                sw.WriteLine(TextCreator(type, msg));
            }
        }

        protected abstract string TextCreator(LogType type, string msg);
    }

    public enum LogType
    {
        Info,
        Warning,
        Error,
        Fatal
    }

    public sealed class LoggersCollection
    {
        public List<Logger> loggers { get; } =  new List<Logger>();
        public ReadOnlyCollection<Logger> Loggers => loggers.AsReadOnly();

        public void Add(params Logger[] loggerCollection)
        {
            CollectionManipulation((Logger item) => 
            {
                bool isTheSameFile = !string.IsNullOrEmpty(item.OutputPath) &&
                   !string.IsNullOrWhiteSpace(item.OutputPath) &&
                   loggers.Any(x=>x.OutputPath == item.OutputPath);
                bool isSecondConsole = item.LoggerName == LoggerTypes.ConsoleLogger.ToString() &&
                   loggers.Any(x=>x.LoggerName == item.LoggerName);

                if(isTheSameFile || isSecondConsole)
                   {
                       return;
                   }

                loggers.Add(item);
            },loggerCollection);
        }
        public void Remove(params Logger[] loggerCollection)
        {
            CollectionManipulation((Logger item)=> loggers.Remove(item),loggerCollection);
        }

        private void CollectionManipulation(Action<Logger> action, params Logger[] loggerCollection)
        {
            if(loggerCollection == null)
            {
                return;
            }

            foreach (var item in loggerCollection)
            {
                action(item);
            }
        }

        public string TestName { get; set; }
        public int TestStep { get; set; }

        public void Log(LogType type, string msg)
        {
            foreach (var item in loggers)
            {
                item.TestName = TestName;
                item.TestStep = TestStep;
                item.Log(type, msg);
            }
        }

        private readonly object locker = new object();
        public void ThreadSaveLog(LogType type, string msg, string testName, int testStep)
        {
            lock(locker)
            {
                TestName = testName;
                TestStep = testStep;
                Log(type,msg);
            }
        }

        public void Log(Exception ex)
        {
            Log(LogType.Error, ExceprionMsg(ex));
        }
        public void ThreadSaveLog(Exception ex, string testName, int testStep)
        {
            ThreadSaveLog(LogType.Error,ExceprionMsg(ex), testName, testStep);
        }

        private string ExceprionMsg(Exception ex)
        {
            return $"Message : {ex.Message} + || Stack trace : {ex.StackTrace}";
        }

    }
}