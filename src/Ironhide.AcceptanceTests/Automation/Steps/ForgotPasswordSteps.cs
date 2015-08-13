using System.Linq;
using FluentAssertions;
using FluentAutomation;
using Starscream.AcceptanceTests.Automation.PageObjects;
using TechTalk.SpecFlow;

namespace Starscream.AcceptanceTests.Automation.Steps
{
    [Binding]
    public class ForgotPasswordSteps : FluentTest
    {
        const string UserEmailAddress = "test@test.com";
        ForgotPasswordPage _forgotPasswordPage;

        [Given(@"a website visitor")]
        public void GivenAWebsiteVisitor()
        {
        }

        [Given(@"the forgot password page")]
        public void GivenTheForgotPasswordPage()
        {
            _forgotPasswordPage = new ForgotPasswordPage(this);
            _forgotPasswordPage.Go();
        }

        [When(@"enter my email address")]
        public void WhenEnterMyEmailAddress()
        {
            _forgotPasswordPage.EnterEmailAddress(UserEmailAddress);
        }

        [When(@"click the submit button")]
        public void WhenClickTheSubmitButton()
        {
            _forgotPasswordPage.ClickSubmitButton();
        }

        [Then(@"I should see a thank you screen")]
        public void ThenIShouldSeeAThankYouScreen()
        {
            _forgotPasswordPage.GetInfoMessage().Should().Contain(UserEmailAddress);
        }

        [Then(@"I should receive an email with a link to reset my password")]
        public void ThenIShouldReceiveAnEmailWithALinkToResetMyPassword()
        {
            _forgotPasswordPage.ReceiveResetEmail().Should().NotBeNull();
        }
    }
}