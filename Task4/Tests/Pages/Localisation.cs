using System.Collections.Generic;

using SeleniumWrapper.Utils;

namespace Tests.Pages
{
    public enum Language
    {
        Ru,
        En
    }

    enum Test_1
    {
        Title
    }

    public enum Test_2
    {
        Action,
        Indie,
    }

    public enum Month
    {
        January,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }

    public enum GenericParams
    {
        Language
    }

    static class LocalisationKeeper
    {
#region Localisation comparation
        private static readonly SeleniumWrapper.Utils.Localisation<Test_1,Language> LocalisationForTest_1 =
            new Dictionary<Test_1,Dictionary<Language,string>>();
        private static readonly SeleniumWrapper.Utils.Localisation<Test_2,Language> LocalisationForTest_2 =
            new Dictionary<Test_2,Dictionary<Language,string>>();
        private static readonly SeleniumWrapper.Utils.Localisation<Month,Language> MonthLocalisation = 
            new Dictionary<Month, Dictionary<Language, string>>();
        public static readonly SeleniumWrapper.Utils.Localisation<GenericParams,Language> LanguageNames = 
            new Dictionary<GenericParams, Dictionary<Language, string>>();

#endregion

        public static string Get(Test_1 param, Language ln)
        {
            return LocalisationForTest_1[param][ln];
        }
        public static string Get(Test_2 param, Language ln)
        {
            return LocalisationForTest_2[param][ln];
        }
        public static string Get(Month month, Language ln)
        {
            return MonthLocalisation[month][ln];
        }

        private static bool tougle = false;
        public static void Configure(string test_1, string test_2,
                                string month, string ln)
        {
            if(tougle)
            {
                return;
            }
            tougle = true;
            LocalisationForTest_1.Deserialization(test_1);
            LocalisationForTest_2.Deserialization(test_2);
            MonthLocalisation.Deserialization(month);
            LanguageNames.Deserialization(ln);
        }
    }
}