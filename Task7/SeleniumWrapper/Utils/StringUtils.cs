using System;
using System.Linq;

namespace SeleniumWrapper.Utils
{
    public static class StringUtils
    {
        public static bool IsWrongString(string s)
        {
            return string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s);
        }
        private static Random random = new Random();
        public static string RandomString(int length = 50, string combineFrom = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            return new string(Enumerable.Repeat(combineFrom, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}