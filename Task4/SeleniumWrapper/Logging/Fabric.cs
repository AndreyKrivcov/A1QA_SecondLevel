using System;
using System.Collections.Generic;

namespace SeleniumWrapper.Logging
{
    public static class LoggerCreator
    {
        private static Dictionary<LoggerTypes, LoggerFabric> loggersCollection = new Dictionary<LoggerTypes, LoggerFabric>
        {
            {LoggerTypes.ConsoleLogger, new ConsoleLoggerCreator()},
            {LoggerTypes.FileLogger, new FileLoggerCreator()}
        };

        public static Logger GetLogger(LoggerTypes type, 
            Func<LogType, string, string, int?, string> textCreator, params object [] inputData)
        {
            return loggersCollection[type].GetLogger(textCreator,inputData);
        }
    }

    internal abstract class LoggerFabric
    {
        public abstract Logger GetLogger(Func<LogType, string, string, int?,string> textCreator, params object [] inputData);
    }

    public enum LoggerTypes
    {
        ConsoleLogger,
        FileLogger
    }
}