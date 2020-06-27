using System;
using System.IO;

namespace SeleniumWrapper.Logging
{
    internal sealed class FileLoggerCreator : LoggerFabric
    {
        public override Logger GetLogger(Func<LogType, string, string, int?,string> textCreator, params object [] inputData)
        {
            if(inputData == null || inputData.Length != 1)
            {
                throw new ArgumentException("Incorrect inputData");
            }

            string path = inputData[0] as string;
            return (textCreator == null ? new FileLogger(path) : new FileLogger(path, textCreator));

        }
    }

    internal sealed class FileLogger : Logger
    {
        public FileLogger(string pathToFile) : this(pathToFile, TextCreator)
        {
        }
        public FileLogger(string pathToFile, Func<LogType, string, string, int?,string> textCreator) : 
            base(()=> CreateStream(pathToFile), LoggerTypes.FileLogger.ToString(), textCreator)
        {
            if(string.IsNullOrEmpty(pathToFile) || string.IsNullOrWhiteSpace(pathToFile))
            {
                throw new ArgumentException("Wrong path to file");
            }
            OutputPath = pathToFile;
        }



        private static string TextCreator(LogType type, string msg, string testName, int? testStep)
        {
            string notification;
            switch (type)
            {
                case LogType.Info: notification = ">"; break;
                case LogType.Warning: notification = "!"; break; 
                case LogType.Error: notification = "!!"; break;
                case LogType.Fatal: notification = "!!!"; break;
                default: return TextCreator(LogType.Info, msg, testName, testStep);
            }

            return $"{notification};{testName};{(testStep.HasValue ? testStep.Value.ToString() : "")};{msg}";
        }

        private static StreamWriter CreateStream(string fileName)
        {
            StreamWriter fileStream;
            if(!File.Exists(fileName))
            {
                fileStream = new StreamWriter(File.Open(fileName, FileMode.Create | FileMode.Append));
                fileStream.WriteLine("Notification;Test name; Test step; Message");
            }
            else
            {
                fileStream = new StreamWriter(File.Open(fileName,FileMode.Append));
            }

            return fileStream;
        }
    }
}