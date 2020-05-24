using System;
using System.Linq;

namespace TestingLibUnitTests
{
    public class HelpfulMethods
    {
        public static object GetPrivateField(string fieldName, object o)
        {
            return o.GetType().GetField(fieldName, System.Reflection.BindingFlags.Instance |
                                            System.Reflection.BindingFlags.NonPublic)
                       .GetValue(o);
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var test = Enumerable.Repeat(chars, length);
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
