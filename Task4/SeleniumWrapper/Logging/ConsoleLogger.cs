using System;
using System.IO;

namespace SeleniumWrapper.Logging
{
    internal sealed class ConsoleLoggerCreator : LoggerFabric
    {
        public override Logger GetLogger(Func<LogType, string, string, int?,string> textCreator, params object [] inputData)
        {
            return (textCreator == null ? new ConsoleLogger() : new ConsoleLogger(textCreator));
        } 
    }

    internal sealed class ConsoleLogger : Logger
    {
        public ConsoleLogger() : this(TextCreator)
        {
        }
        public ConsoleLogger(Func<LogType, string, string, int?,string> textCreator) : 
            base(()=>new StreamWriter(Console.OpenStandardOutput()), LoggerTypes.ConsoleLogger.ToString(), textCreator)
        {
            defaultColor = Console.ForegroundColor;
        }

        private readonly ConsoleColor defaultColor;

        private static string TextCreator(LogType type, string msg, string testName, int? testStep)
        {
            return string.Format(string.Concat(">>> {0} \t | \t", $"Test \t\"{testName}\" \t | \t Step \t#{testStep?? testStep.Value} \t ||\t{msg}"), type.ToString());
        }

        public override void Log(LogType type, string msg)
        {
            ConsoleCollor(type);
            base.Log(type,msg);
            Console.ForegroundColor = defaultColor;
        }

        private void ConsoleCollor(LogType type)
        {
            switch (type)
            {                
                case LogType.Info : Console.ForegroundColor = ConsoleColor.Gray; break;
                case LogType.Warning : Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                case LogType.Error : Console.ForegroundColor = ConsoleColor.Red; break;
                case LogType.Fatal : Console.ForegroundColor = ConsoleColor.DarkRed; break;
                default: Console.ForegroundColor = defaultColor; break;
            }
        }
    }
}