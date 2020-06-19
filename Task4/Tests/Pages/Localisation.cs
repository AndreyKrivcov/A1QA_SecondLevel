using System.Collections.Generic;

using SeleniumWrapper.Utils;

namespace Tests.Pages
{
    enum Language
    {
        Ru,
        En
    }

    enum Test_1
    {
        Title,yyy
    }

    public enum Test_2
    {
        Action,
        Indie
    }

    static class LocalisationKeeper
    {
#region Localisation comparation
        private static readonly SeleniumWrapper.Utils.Localisation<Test_1,Language> LocalisationForTest_1 
            = new Dictionary<Test_1,Dictionary<Language,string>>
        {
            {Test_1.Title, new Dictionary<Language, string>
            {
                {Language.Ru, "Добро пожаловать в Steam"},
                {Language.En, "Welcome to Steam"}
            }}
        };
        private static readonly SeleniumWrapper.Utils.Localisation<Test_2,Language> LocalisationForTest_2 
        = new Dictionary<Test_2,Dictionary<Language,string>> 
        {
            {Test_2.Action, new Dictionary<Language,string>
            {
                {Language.Ru, "Экшен"},
                {Language.En, "Action"}
            }},
            {Test_2.Indie, new Dictionary<Language, string>
            {
                {Language.Ru, "Indie"},
                {Language.En, "Инди"}
            }}
        };
        public static Dictionary<Language,string> LanguageNames { get; }= new Dictionary<Language, string>
        {
            {Language.Ru, "Русский (Russian)"},
            {Language.En, "English (английский)"}
        };
#endregion

        public static string Get(Test_1 param, Language ln)
        {
            return LocalisationForTest_1[param][ln];
        }
        public static string Get(Test_2 param, Language ln)
        {
            return LocalisationForTest_2[param][ln];
        }
    }
}