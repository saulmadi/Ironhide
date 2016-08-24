using System;

namespace Ironhide.Common
{
    public interface ITimeProvider
    {
        DateTime Now();
    }
}