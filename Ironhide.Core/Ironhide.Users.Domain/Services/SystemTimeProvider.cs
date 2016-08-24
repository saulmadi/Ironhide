using System;
using Ironhide.Common;

namespace Ironhide.Users.Domain.Services
{
    public class SystemTimeProvider : ITimeProvider
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}