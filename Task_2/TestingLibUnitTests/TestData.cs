using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp1;

namespace TestingLibUnitTests
{
    public class TestData
    {
        #region Data

        private static readonly string[] filePaths = new[] { "", "C://"};
        public static string FileName => "MyFile";
        private static readonly string[] extention = new[] { "", ".txt" };
        public static string[] Content => new string[] { "", null, "This is non empry content" };
        public static int[] FileStoargeSize => new[] { 0, 200 };

        #endregion

        #region Parameters generators

        public static List<string> GetFileNames()
        {
            List<string> ans = new List<string>();

            foreach (var path in filePaths)
            {
                foreach (var extention_item in extention)
                {
                    ans.Add(System.IO.Path.Combine(path, $"{FileName}{extention_item}"));
                }
            }
            ans.Add(null);
            ans.Add("");

            return ans;
        }

        public static List<KeyValuePair<string, string>> GetInputData()
        {
            List<KeyValuePair<string, string>> ans = new List<KeyValuePair<string, string>>();

            List<string> files = GetFileNames();

            foreach (var file in files)
            {
                foreach (var content in Content)
                {
                    if (!ans.Any(x => x.Key == file && x.Value == content))
                    {
                        ans.Add(new KeyValuePair<string, string>(file, content));
                    }
                }
            }

            return ans;
        }

        private static Random random = new Random();
        public static List<File> GetFileCollection(int filesQuality, int maxPossibleStringLength)
        {

            List<File> files = new List<File>();
            while (filesQuality-- > 0)
            {
                files.Add(new File($"{HelpfulMethods.RandomString(5)}.txt", HelpfulMethods.RandomString(random.Next(maxPossibleStringLength))));
            }

            return files;
        }

        #endregion
    }

}
