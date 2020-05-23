using NUnit.Framework;
using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestingLibUnitTests
{
    [TestFixture()]
    public class FileTest
    {
        [TestOf("File.getSize()")]
        [TestCaseSource(typeof(TestData), "Content")]
        public void Test_getSize(string content)
        {
            if (content != null)
            {
                File file = new File(TestData.FileName, content);

                double expect = (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content) ? 0 : content.Length / 2);
                double actual = file.getSize();

                Assert.AreEqual(expect, actual, $"Passed content = {content} | passed file name = {TestData.FileName}");
            }
            else
            {
                Assert.Throws<NullReferenceException>(() => new File(TestData.FileName, content));
            }
        }

        [TestOf("File.getFilename()")]
        [TestCaseSource(typeof(TestData), "GetFileNames")]
        public void Test_getFilename(string fileName)
        {
            if (fileName != null)
            {
                File file = new File(fileName, "");

                string actual = file.getFilename();
                Assert.AreEqual(fileName, actual);
            }
            else
            {
                Assert.Throws<NullReferenceException>(()=>new File(fileName, ""));
            }
        }

        private object GetPrivateField(string fieldName, object o)
        {
            return o.GetType().GetField(fieldName, System.Reflection.BindingFlags.Instance | 
                                            System.Reflection.BindingFlags.NonPublic)
                       .GetValue(o);
        }

        [TestOf("new File(string filename, string content)")]
        [TestCaseSource(typeof(TestData), "GetInputData")]
        public void Test_Constructor(KeyValuePair<string, string> data)
        {
            if (data.Value == null || data.Key == null)
            {
                Assert.Throws<NullReferenceException>(()=>new File(data.Key, data.Value));
            }
            else
            {
                var file = new File(data.Key, data.Value);

                string expect_extention = new System.IO.FileInfo(data.Key).Extension;
                if(!string.IsNullOrEmpty(expect_extention))
                {
                    expect_extention = expect_extention.Split('.')[1];
                }
                string expect_fileName = data.Key;
                string expect_content = data.Value;
                double expect_size = data.Value.Length/2;

                string actual_extention = GetPrivateField("extension", file).ToString();
                string actual_fileName = GetPrivateField("filename", file).ToString();
                string actual_content = GetPrivateField("content", file).ToString();
                double actual_size = Convert.ToDouble(GetPrivateField("size", file));

                Assert.AreEqual(expect_extention, actual_extention);
                Assert.AreEqual(expect_fileName, actual_fileName);
                Assert.AreEqual(expect_content, actual_content);
                Assert.AreEqual(expect_size, actual_size);
            }
        }

    }

    public class TestData
    {

        private static readonly string[] filePaths = new[] { "", "C://" };
        public static string FileName => "MyFile";
        private static readonly string[] extention = new[] { "", ".txt" };
        public static List<string> Content => new List<string> { "", null, "This is non empry content" };
            
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

        public static List<KeyValuePair<string,string>> GetInputData()
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
    }


}
