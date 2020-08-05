using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SeleniumWrapper.Logging
{
    public abstract class Logger
    {
        protected Logger(Func<StreamWriter> streamCreator, string loggerName, 
            Func<LogType, string, string, int?,string> textCreator)
        {
            this.streamCreator = streamCreator;
            LoggerName = loggerName;
            this.textCreator = textCreator;
        }

        protected readonly Func<StreamWriter> streamCreator;
        protected readonly Func<LogType, string, string, int?,string> textCreator;

        public virtual string TestName { get; set; }
        public virtual int? TestStep { get; set; }
        public virtual string OutputPath{ get; protected set;}
        public string LoggerName { get; }

        public virtual void Log(LogType type, string msg)
        {
            using(StreamWriter sw = streamCreator())
            {
                sw.WriteLine(textCreator(type, msg, TestName, TestStep));
            }
        }
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

        public void Log(LogType type, string msg, string testName, int? testStep)
        {
            foreach (var item in loggers)
            {
                item.TestName = testName;
                item.TestStep = testStep;
                item.Log(type, msg);
            }
        }

        private readonly object locker = new object();
        public void ThreadSaveLog(LogType type, string msg, string testName, int? testStep)
        {
            lock(locker)
            {
                Log(type,msg, testName, testStep);
            }
        }

        public void Log(Exception ex, string testName, int? testStep)
        {
            Log(LogType.Error, ExceprionMsg(ex),testName, testStep);
        }
        public void ThreadSaveLog(Exception ex, string testName, int? testStep)
        {
            ThreadSaveLog(LogType.Error,ExceprionMsg(ex), testName, testStep);
        }

        private string ExceprionMsg(Exception ex)
        {
            return $"Message : {ex.Message} + || Stack trace : {ex.StackTrace}";
        }

        public void StartTest(string testName)
        {
            Log(LogType.Info, $"================================ {testName} Started ================================", testName,null);
        } 
        public void EndTest(string testName)
        {
            Log(LogType.Info, $"================================ {testName} Finished ================================", testName,null);
        } 
        public void ThreadSaveStartTest(string testName)
        {
            lock(locker)
                StartTest(testName);
        } 
        public void ThreadSaveEndTest(string testName)
        {
            lock(locker)
                EndTest(testName);
        } 

    }
}