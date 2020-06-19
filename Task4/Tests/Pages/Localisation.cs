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
        Indie,
    }

    enum Month
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
            }},
        };
        private static readonly SeleniumWrapper.Utils.Localisation<Month,Language> MonthLocalisation = 
            new Dictionary<Month, Dictionary<Language, string>>
        {
            {Month.January, new Dictionary<Language, string>
            {
                {Language.Ru, "января"},
                {Language.En, "January"}
            }},
            {Month.February, new Dictionary<Language, string>
            {
                {Language.Ru, "февраля"},
                {Language.En, "February"}
            }},
            {Month.March, new Dictionary<Language, string>
            {
                {Language.Ru, "марта"},
                {Language.En, "March"}
            }},
            {Month.April, new Dictionary<Language, string>
            {
                {Language.Ru, "апреля"},
                {Language.En, "April"}
            }},
            {Month.May, new Dictionary<Language, string>
            {
                {Language.Ru, "мая"},
                {Language.En, "May"}
            }},
            {Month.June, new Dictionary<Language, string>
            {
                {Language.Ru, "июня"},
                {Language.En, "June"}
            }},
            {Month.July, new Dictionary<Language, string>
            {
                {Language.Ru, "июля"},
                {Language.En, "July"}
            }},
            {Month.August, new Dictionary<Language, string>
            {
                {Language.Ru, "августа"},
                {Language.En, "August"}
            }},
            {Month.September, new Dictionary<Language, string>
            {
                {Language.Ru, "сентября"},
                {Language.En, "September"}
            }},
            {Month.October, new Dictionary<Language, string>
            {
                {Language.Ru, "октября"},
                {Language.En, "October"}
            }},
            {Month.November, new Dictionary<Language, string>
            {
                {Language.Ru, "ноября"},
                {Language.En, "November"}
            }},
            {Month.December, new Dictionary<Language, string>
            {
                {Language.Ru, "декабря"},
                {Language.En, "December"}
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