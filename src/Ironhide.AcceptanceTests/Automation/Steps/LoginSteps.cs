using FluentAssertions;
using FluentAutomation;
using Starscream.AcceptanceTests.Automation.PageObjects;
using TechTalk.SpecFlow;

namespace Starscream.AcceptanceTests.Automation.Steps
{
    [Binding]
    public class LoginSteps : FluentTest
    {
        LoginPage _loginPage;

        [Given(@"I have navigated to the login page")]
        public void GivenIHaveNavigatedToTheLoginPage()
        {            
            _loginPage = new LoginPage(this);
            _loginPage.Go();
        }

        [When(@"I have entered ""(.*)"" into the email address field")]
        public void WhenIHaveEnteredIntoTheEmailAddressField(string p0)
        {
            _loginPage.EnterEmailAddress(p0);
        }

        [When(@"I have entered ""(.*)"" into the password field")]
        public void WhenIHaveEnteredIntoThePasswordField(string p0)
        {
            _loginPage.EnterPassword(p0);
        }

        [When(@"I have clicked the login button")]
        public void WhenIHaveClickedTheLoginButton()
        {
            _loginPage.ClickSubmitButton();
        }

        [Then(@"I should see the home screen")]
        public void ThenIShouldSeeTheHomeScreen()
        {
            _loginPage.GetTitle().Should().Contain("Home");
        }

        [Then(@"I should see the error ""(.*)""")]
        public void ThenIShouldSeeTheError(string p0)
        {
            _loginPage.GetErrorMessage().Should().Contain("Invalid Login");
        }        
    }
}