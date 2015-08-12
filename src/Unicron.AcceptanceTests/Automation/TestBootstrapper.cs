using FluentAutomation;
using TechTalk.SpecFlow;

namespace Starscream.AcceptanceTests.Automation
{
    [Binding]
    public class TestBootstrapper
    {
        [BeforeTestRun]
        public static void BootstrapFluentAutomation()
        {
            SeleniumWebDriver.Bootstrap(
                SeleniumWebDriver.Browser.Firefox
                );
        }
    }
}