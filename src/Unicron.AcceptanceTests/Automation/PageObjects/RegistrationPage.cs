using FluentAutomation;

namespace Starscream.AcceptanceTests.Automation.PageObjects
{
    public class RegistrationPage : PageObject<RegistrationPage>
    {
        const string EmailInput = "#email";
        const string PasswordInput1 = "#password1";
        const string PasswordInput2 = "#password2";
        const string NameInput = "#name";
        const string PhoneNumberInput = "#phoneNumber";
        const string SubmitButton = "button[type=submit]";
        const string ConfirmationMessage = "#confirmation";

        public RegistrationPage(FluentTest test)
            : base(test)
        {
            Url = TargetEnvironment.GetApplicationUrl("/#/register");
            At = () => I.Expect.Exists(EmailInput);
        }

        public RegistrationPage EnterEmailAddress(string emailAddress)
        {
            I.WaitUntil(() => I.Assert.Exists(EmailInput));            
            I.Enter(emailAddress).In(EmailInput);
            return this;
        }

        public RegistrationPage EnterPassword(string password)
        {
            I.WaitUntil(() => I.Assert.Exists(PasswordInput1));
            I.Enter(password).In(PasswordInput1);
            I.Enter(password).In(PasswordInput2);
            return this;
        }

        public RegistrationPage EnterOtherRequiredFields(string name, string phoneNumber)
        {
            I.Enter(name).In(NameInput);
            I.Enter(phoneNumber).In(PhoneNumberInput);
            return this;
        }

        public RegistrationPage ClickSubmitButton()
        {
            I.Click(SubmitButton);
            return this;
        }

        public string GetConfirmation()
        {
            I.WaitUntil(() => I.Find(ConfirmationMessage).Element.Text != "");
            return I.Find(ConfirmationMessage).Element.Text;
        }
    }
}