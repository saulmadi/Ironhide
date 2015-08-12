using System;
using AcklenAvenue.Email;
using Unicron.Users.Domain;

namespace Unicron.Web.Api.emails
{
    public class PasswordResetEmailTemplate : IEmailBodyTemplate, IEmailSubjectTemplate
    {
        public Type ForType
        {
            get { return typeof (PasswordResetEmail); }
        }

        public string BodyTemplate
        {
            get
            {
                return
                    "A request was made to reset your password. If you didn't make this request, you can ignore this email. If you need to reset your password, click this link to get started: <a href='@Model.ResetUrl'>@Model.ResetUrl</a>.";
            }
        }

        public string SubjectTemplate
        {
            get { return "Password Reset"; }
        }
    }
       
}