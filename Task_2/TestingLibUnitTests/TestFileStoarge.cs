using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp1;
using NUnit.Framework;

namespace TestingLibUnitTests
{
    [TestFixture()]
    public class TestFileStoarge
    {
        [Test]
        public void Test_CompareConstructors()
        {
            FileStorage defaultConstructor = new FileStorage();
            int expected_maxSize = Convert.ToInt32(HelpfulMethods.GetPrivateField("maxSize", defaultConstructor));
            int expected_availableSize = Convert.ToInt32(HelpfulMethods.GetPrivateField("availableSize", defaultConstructor));

            FileStorage userConstructor = new FileStorage(expected_availableSize);
            int actual_maxSize = Convert.ToInt32(HelpfulMethods.GetPrivateField("maxSize", userConstructor));
            int actual_availableSize = Convert.ToInt32(HelpfulMethods.GetPrivateField("availableSize", userConstructor));

            Assert.AreEqual(expected_maxSize, actual_maxSize);
            Assert.AreEqual(expected_availableSize, actual_availableSize);
        }

        [TestOf("FileStorage(int size)")]
        [TestCaseSource(typeof(TestData), "FileStoargeSize")]
        public void Test_UserConstructor(int size)
        {
            FileStorage userConstructor = new FileStorage(size);
            int actual_maxSize = Convert.ToInt32(HelpfulMethods.GetPrivateField("maxSize", userConstructor));
            int actual_availableSize = Convert.ToInt32(HelpfulMethods.GetPrivateField("availableSize", userConstructor));

            Assert.AreEqual(size, actual_maxSize);
            Assert.AreEqual(size, actual_availableSize);
        }

        [TestOf("FileStorage.write(File file)")]
        [TestCase(10,5)]
        public void Test_CorrectWriteCollection(int filesQuality, int maxPossibleFileContentSize)
        {
            var files = TestData.GetFileCollection(filesQuality, maxPossibleFileContentSize);

            GetFilledStorage(files);
        }

        [TestOf("FileStorage.write(File file)")]
        [TestCase(10)]
        public void Test_WriteFileGraterThanMaxPossibleSize(int maxPossibleFileContentSize)
        {
            File file = new File(TestData.FileName, HelpfulMethods.RandomString(maxPossibleFileContentSize));
            FileStorage storage = new FileStorage(Convert.ToInt32(file.getSize() - 1));
            Assert.False(storage.write(file));
        }

        [TestOf("FileStorage.write(File file)")]
        [TestCase(10, 5)]
        public void Test_WriteOversizedCollection(int filesQuality, int maxPossibleFileContentSize)
        {
            var expected_files = TestData.GetFileCollection(filesQuality, maxPossibleFileContentSize);
            FileStorage storage = GetFilledStorage(expected_files);
            var oversized_files = TestData.GetFileCollection(filesQuality, maxPossibleFileContentSize);

            oversized_files.ForEach(x => Assert.False(storage.write(x)));
        }

        [TestOf("FileStorage.getFiles()")]
        [TestCase(10, 5)]
        public void Test_ReadCorrectWritedCollection(int filesQuality, int maxPossibleFileContentSize)
        {
            var files = TestData.GetFileCollection(filesQuality, maxPossibleFileContentSize);

            var actual_files = GetFilledStorage(files).getFiles();
            files.ForEach(x => Assert.True(actual_files.Contains(x)));
        }

        [TestOf("FileStorage.isExists()")]
        [Test]
        public void Test_isExists()
        {
            var data = GetFilledStorage();

            Assert.True(data.Value.isExists(data.Key.getFilename()));
            Assert.False(data.Value.isExists($"Wrong name of the file { data.Key.getFilename() }"));
        }

        [TestOf("FileStorage.delete(string fileName)")]
        [Test]
        public void Test_deleteTwice()
        {
            var data = GetFilledStorage();

            Assert.True(data.Value.delete(data.Key.getFilename()));
            Assert.False(data.Value.delete(data.Key.getFilename()));
        }

        [TestOf("FileStorage.delete(string fileName)")]
        [Test]
        public void Test_deleteCorrectAndWrongFiles()
        {
            var data = GetFilledStorage();

            Assert.True(data.Value.delete(data.Key.getFilename()));
            data.Value.write(data.Key);

            Assert.False(data.Value.delete($"Wrong name of the file { data.Key.getFilename() }"));
        }

        [TestOf("FileStorage.getFile(string fileName)")]
        [Test]
        public void Test_getFile()
        {
            var data = GetFilledStorage();

            Assert.AreSame(data.Key, data.Value.getFile(data.Key.getFilename()));
            Assert.Null(data.Value.getFile($"Wrong name of the file { data.Key.getFilename() }"));
        }

        #region Helpful methods
        private KeyValuePair<File,FileStorage> GetFilledStorage()
        {
            File file = new File(TestData.FileName, "");
            FileStorage storage = new FileStorage(Convert.ToInt32(file.getSize()));
            storage.write(file);

            return new KeyValuePair<File,FileStorage>(file, storage);
        }
        private FileStorage GetFilledStorage(List<File> files)
        {
            FileStorage storage = new FileStorage(Convert.ToInt32(files.Sum(x => x.getSize())));
            files.ForEach(x => Assert.True(storage.write(x)));

            return storage;
        }
        #endregion
    }
}
