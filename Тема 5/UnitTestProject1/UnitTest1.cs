using Microsoft.VisualStudio.TestTools.UnitTesting;
using DictionaryLib;
using System.IO;
using System;

namespace DictionaryTests
{
    [TestClass]
    public class SlovarTests
    {
        private string testFilePath = "test_dict.txt";

        [TestInitialize]
        public void Setup()
        {
            File.WriteAllLines(testFilePath, new string[] {
                "казак", "шалаш", "арена", "атом", "тест", "А", "топот"
            }, System.Text.Encoding.UTF8);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(testFilePath)) File.Delete(testFilePath);
        }

        [TestMethod]
        public void TestLoadDictionary()
        {
            var slovar = new Slovar(testFilePath);
            Assert.AreEqual(7, slovar.Count);
        }

        [TestMethod]
        public void TestAddDuplicate()
        {
            var slovar = new Slovar(testFilePath);
            int initialCount = slovar.Count;
            bool result = slovar.AddWord("казак");
            Assert.IsFalse(result);
            Assert.AreEqual(initialCount, slovar.Count);
        }

        [TestMethod]
        public void TestAddUnique()
        {
            var slovar = new Slovar(testFilePath);
            int initialCount = slovar.Count;
            bool result = slovar.AddWord("уникальное");
            Assert.IsTrue(result);
            Assert.AreEqual(initialCount + 1, slovar.Count);
        }

        [TestMethod]
        public void TestVariant15_AnyLetter()
        {
            var slovar = new Slovar(testFilePath);
            var results = slovar.SearchVariant15(null);

            Assert.IsTrue(results.Contains("казак"));
            Assert.IsTrue(results.Contains("шалаш"));
            Assert.IsTrue(results.Contains("арена"));
            Assert.IsTrue(results.Contains("тест"));
            Assert.IsTrue(results.Contains("А"));
            Assert.IsTrue(results.Contains("топот"));
            Assert.IsFalse(results.Contains("атом"));
        }

        [TestMethod]
        public void TestVariant15_SpecificLetter()
        {
            var slovar = new Slovar(testFilePath);
            var results = slovar.SearchVariant15('а');

            Assert.IsTrue(results.Contains("арена"));
            Assert.IsTrue(results.Contains("А"));
            Assert.IsFalse(results.Contains("казак"));
            Assert.IsFalse(results.Contains("тест"));
        }

        [TestMethod]
        public void TestLevenshtein()
        {
            var slovar = new Slovar(testFilePath);
            var results = slovar.FuzzySearch("тест", 1);
            Assert.IsTrue(results.Contains("тест"));
        }
    }
}