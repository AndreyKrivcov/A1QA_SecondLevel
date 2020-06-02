using NUnit.Framework;

namespace Task_3
{
    public class SeleniumTest
    {
        [SetUp]
        public void Setup()
        {
            config = ConfigSerializer.Deserialize();
            if(config == null)
            {
                config = new Config();
                ConfigSerializer.Serialize(config);
            }
        }

        Config config;

        [Test]
        public void Test_YandexMarket()
        {
        }
    }
}
