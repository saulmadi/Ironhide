using System;
using FluentAssertions;
using FluentAutomation;
using Starscream.AcceptanceTests.Automation.PageObjects;
using TechTalk.SpecFlow;

namespace Starscream.AcceptanceTests.Automation.Steps
{
    [Binding]
    public class UserCanRegisterSteps : FluentTest
    {
        RegistrationPage _page;

        [Given(@"I have navigated to the registration page")]
        public void GivenIHaveNavigatedToTheRegistrationPage()
        {
            _page = new RegistrationPage(this);
            _page.Go();
        }

        [When(@"I have entered a random email address into the email address field")]
        public void WhenIHaveEnteredARandomEmailAddressIntoTheEmailAddressField()
        {
            _page.EnterEmailAddress(string.Format("test{0}@test.com", new Random().Next(999999)));
        }

        [When(@"I have filled in the rest of the required fields")]
        public void WhenIHaveFilledInTheRestOfTheRequiredFields()
        {
            _page.EnterPassword("somepassword");
            _page.EnterOtherRequiredFields("Test User Name", "615-669-8230");
        }

        [When(@"I have clicked the register button")]
        public void WhenIHaveClickedTheRegisterButton()
        {
            _page.ClickSubmitButton();
        }

        [Then(@"I should see the confirmation screen")]
        public void ThenIShouldSeeTheConfirmationScreen()
        {
            _page.GetConfirmation().Should().Contain("Thanks");
        }
    }
}