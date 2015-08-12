using System;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using TechTalk.SpecFlow;

namespace Starscream.AcceptanceTests
{
    public static class Global
    {
        public static IWebDriver GetCurrentWebDriver()
        {
            try
            {
                return FeatureContext.Current.Get<IWebDriver>();
            }
            catch (Exception)
            {
                var currentWebDriver = new FirefoxDriver();
                FeatureContext.Current.Set<IWebDriver>(currentWebDriver);
                return currentWebDriver;
            }
        }

        public static string GetApplicationUrl(string route)
        {
            var baseUrl = ConfigurationManager.AppSettings["baseUrl"];
            return baseUrl + route;
        }
    }
}