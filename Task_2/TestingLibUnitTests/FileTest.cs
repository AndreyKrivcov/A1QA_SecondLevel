using NUnit.Framework;
using ConsoleApp1;
using System;
using System.Collections.Generic;

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
                Assert.That(()=>new File(TestData.FileName, content), Throws.Exception);
            }
        }

        [TestOf("File.getFilename()")]
        [TestCaseSource(typeof(TestData), "GetFileNames")]
        public void Test_getFilename(string fileName)
        {
            if(!string.IsNullOrEmpty(fileName) || !string.IsNullOrWhiteSpace(fileName))
            {
                File file = new File(fileName, "");

                string actual = file.getFilename();
                Assert.AreEqual(fileName, actual);
            }
            else
            {
                Assert.That(() => new File(fileName, ""), Throws.Exception);
            }
        }

        [TestOf("new File(string filename, string content)")]
        [TestCaseSource(typeof(TestData), "GetInputData")]
        public void Test_Constructor(KeyValuePair<string, string> data)
        {
            if (data.Value == null || string.IsNullOrEmpty(data.Key) || string.IsNullOrWhiteSpace(data.Key))
            {
                Assert.That(() => new File(data.Key, data.Value), Throws.Exception);
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

                string actual_extention = HelpfulMethods.GetPrivateField("extension", file).ToString();
                string actual_fileName = HelpfulMethods.GetPrivateField("filename", file).ToString();
                string actual_content = HelpfulMethods.GetPrivateField("content", file).ToString();
                double actual_size = Convert.ToDouble(HelpfulMethods.GetPrivateField("size", file));

                Assert.AreEqual(expect_extention, actual_extention);
                Assert.AreEqual(expect_fileName, actual_fileName);
                Assert.AreEqual(expect_content, actual_content);
                Assert.AreEqual(expect_size, actual_size);
            }
        }
    }
}
