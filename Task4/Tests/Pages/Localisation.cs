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
        Action,
        Title,
        DownloadPage
    }

    static class LocalisationKeeper
    {
        public static SeleniumWrapper.Utils.Localisation<Test_1,Language> LocalisationForTest_1 { get; } = new Dictionary<Test_1,Dictionary<Language,string>>
        {
            {Test_1.Action, new Dictionary<Language, string>
            {
                {Language.Ru, "Русский (Russian)"},
                {Language.En, "English (английский)"}
            }},
            {Test_1.Title, new Dictionary<Language, string>
            {
                {Language.Ru, "Добро пожаловать в Steam"},
                {Language.En, "Welcome to Steam"}
            }}
        };

        public static Dictionary<Language,string> LanguageNames { get; }= new Dictionary<Language, string>
            {
                {Language.Ru, "Русский (Russian)"},
                {Language.En, "English (английский)"}
            };
    }
}