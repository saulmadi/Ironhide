using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Starscream.AcceptanceTests
{
    public abstract class PageBase
    {
        protected IWebDriver WebDriver;
        protected string Url;

        protected PageBase(string pageRoute)
        {
            WebDriver = Global.GetCurrentWebDriver();
            Url = Global.GetApplicationUrl(pageRoute);
        }

        public void NavigateToPage()
        {
            WebDriver.Navigate().GoToUrl(Url);
            PageFactory.InitElements(WebDriver, this);
        }

        public string GetPageTitle()
        {
            return WebDriver.Title;
        }

        public void ClosePage()
        {
            WebDriver.Quit();
        }
    }
}