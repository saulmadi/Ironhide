using System;
using Ironhide.Common;

namespace Ironhide.Login.Domain.Services
{
    public class SystemTimeProvider : ITimeProvider
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}