using NUnit.Framework;

namespace Task_3
{
    public class SeleniumTest
    {
        [SetUp]
        public void Setup()
        {
            config = configSerializer.Deserialize();
            if(config == null)
            {
                config = new Config();
                configSerializer.Serialize(config);
            }
        }

        Config config;
        ConfigSerializer configSerializer = ConfigSerializer.Instance();

        [Test]
        public void Test_YandexMarket()
        {
        }
    }
}
