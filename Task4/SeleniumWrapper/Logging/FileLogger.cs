using System.IO;

namespace SeleniumWrapper.Logging
{
    internal sealed class FileLoggerCreator : LoggerFabric
    {
        public override Logger GetLogger(string pathToFile) => new FileLogger(pathToFile);
    }

    internal sealed class FileLogger : Logger
    {
       public FileLogger(string pathToFile) : base(()=> CreateStream(pathToFile), LoggerTypes.FileLogger.ToString())
       {
           OutputPath = pathToFile;
       }

        protected override string TextCreator(LogType type, string msg)
        {
            string notification;
            switch (type)
            {
                case LogType.Info: notification = ">"; break;
                case LogType.Warning: notification = "!"; break; 
                case LogType.Error: notification = "!!"; break;
                case LogType.Fatal: notification = "!!!"; break;
                default: return TextCreator(LogType.Info, msg);
            }

            return $"{notification};{TestName};{TestStep};{msg}";
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