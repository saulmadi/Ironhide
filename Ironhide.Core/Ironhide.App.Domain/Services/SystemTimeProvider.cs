using System;
using Ironhide.Common;

namespace Ironhide.App.Domain.Services
{
    public class SystemTimeProvider : ITimeProvider
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}