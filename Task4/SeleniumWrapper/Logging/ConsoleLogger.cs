using System;
using System.IO;

namespace SeleniumWrapper.Logging
{
    internal sealed class ConsoleLoggerCreator : LoggerFabric
    {
        public override Logger GetLogger(string pathToFile) => new ConsoleLogger();
    }

    internal sealed class ConsoleLogger : Logger
    {
        public ConsoleLogger() : base(()=>new StreamWriter(Console.OpenStandardOutput()), LoggerTypes.ConsoleLogger.ToString())
        {
            defaultColor = Console.ForegroundColor;
        }

        private readonly ConsoleColor defaultColor;

        protected override string TextCreator(LogType type, string msg)
        {
            return string.Format(string.Concat(">>> {0} \t | \t", $"Test \t\"{TestName}\" \t | \t Step \t#{TestStep} \t ||\t{msg}"), type.ToString());
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