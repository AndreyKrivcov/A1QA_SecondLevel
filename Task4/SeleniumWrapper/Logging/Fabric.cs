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

        public static Logger GetLogger(LoggerTypes type, string pathToFile) => loggersCollection[type].GetLogger(pathToFile);
    }

    internal abstract class LoggerFabric
    {
        public abstract Logger GetLogger(string pathToFile);
    }

    public enum LoggerTypes
    {
        ConsoleLogger,
        FileLogger
    }
}