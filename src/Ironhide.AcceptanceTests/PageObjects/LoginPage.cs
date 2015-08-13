using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Starscream.AcceptanceTests.PageObjects
{
    public class LoginPage : PageBase
    {
        [FindsBy(How = How.CssSelector, Using = "div.alert-danger")]
        IWebElement _errorMessage;

        [FindsBy(How = How.CssSelector, Using = "input[type=email]")]
        IWebElement _emailAddress;

        [FindsBy(How = How.CssSelector, Using = "input[type=password]")]
        IWebElement _password;

        [FindsBy(How = How.CssSelector, Using = "button[type=submit]")]
        IWebElement _submitButton;

        public LoginPage()
            : base("/")
        {
        }

        public LoginPage EnterEmailAddress(string emailAddress)
        {
            _emailAddress.SendKeys(emailAddress);
            return this;
        }

        public LoginPage EnterPassword(string password)
        {
            _password.SendKeys(password);
            return this;
        }

        public LoginPage ClickSubmitButton()
        {
            _submitButton.Click();
            return this;
        }

        public string GetErrorMessage()
        {
            return _errorMessage.Text;
        }
    }
}