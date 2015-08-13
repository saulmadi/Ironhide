using FluentAssertions;
using Starscream.AcceptanceTests.PageObjects;
using TechTalk.SpecFlow;

namespace Starscream.AcceptanceTests.Steps
{
    public class LoginSteps
    {
        LoginPage _loginPage;

        [Given(@"I have navigated to the login page")]
        public void GivenIHaveNavigatedToTheLoginPage()
        {
            _loginPage = new LoginPage();
            _loginPage.NavigateToPage();
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
            _loginPage.GetPageTitle().Should().Contain("Home");
        }

        [Then(@"I should see the error ""(.*)""")]
        public void ThenIShouldSeeTheError(string p0)
        {
            _loginPage.GetErrorMessage().Should().Contain("Invalid Login");
        }

    }
}
