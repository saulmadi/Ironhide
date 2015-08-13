using System;
using System.Collections.Generic;
using System.Linq;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Entities;
using Nancy.Security;

namespace Ironhide.Web.Api.Infrastructure.Authentication
{
    public class LoggedInUserIdentity : IUserIdentity
    {
        public LoggedInUserIdentity(IUserSession userSession)
        {
            UserSession = userSession;
        }

        public IUserSession UserSession { get; private set; }

        #region IUserIdentity Members

        public string UserName
        {
            get
            {
                if (UserSession is UserLoginSession)
                {
                    User executor = ((UserLoginSession) UserSession).User;
                    if (executor == null)
                    {
                        throw new Exception("The user should not be null on the user session.");
                    }
                    return executor.Email;
                }
                return null;
            }
        }

        public IEnumerable<string> Claims
        {
            get
            {
                if (UserSession is UserLoginSession)
                {
                    User executor = ((UserLoginSession)UserSession).User;
                    if (executor == null)
                    {
                        throw new Exception("The user should not be null on the user session.");
                    }
                    return executor.UserRoles.Select(x => x.Description);
                }
                return null;


            }
        }

        #endregion
    }
}