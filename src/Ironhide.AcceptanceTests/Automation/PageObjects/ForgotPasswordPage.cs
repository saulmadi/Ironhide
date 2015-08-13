using System.Linq;
using FluentAutomation;
using netDumbster.smtp;

namespace Starscream.AcceptanceTests.Automation.PageObjects
{
    public class ForgotPasswordPage : PageObject<ForgotPasswordPage>
    {
        const string EmailInput = "input[type=email]";
        const string SubmitButton = "button[type=submit]";
        const string InfoMessage = "div.alert-info";
        readonly SimpleSmtpServer _server;

        public ForgotPasswordPage(FluentTest test) : base(test)
        {
            Url = TargetEnvironment.GetApplicationUrl("/#/forgot-password");
            At = () => I.Expect.Exists(EmailInput);

            _server = SimpleSmtpServer.Start(25);
        }

        public ForgotPasswordPage EnterEmailAddress(string emailAddress)
        {
            I.WaitUntil(() => I.Assert.Exists(EmailInput));
            I.Enter(emailAddress).In(EmailInput);
            return this;
        }

        public ForgotPasswordPage ClickSubmitButton()
        {
            I.Click(SubmitButton);
            I.WaitUntil(() => I.Find(InfoMessage).Element.Text != "");
            return this;
        }

        public string GetInfoMessage()
        {
            return I.Find(InfoMessage).Element.Text;
        }


        public SmtpMessage ReceiveResetEmail()
        {
            SmtpMessage resetEmailReceived =
                _server.ReceivedEmail.FirstOrDefault(
                    x => x.ToAddresses[0].Address == "test@test.com" && x.Headers["Subject"] == "Password Reset");

            _server.Stop();

            return resetEmailReceived;
        }
    }
}