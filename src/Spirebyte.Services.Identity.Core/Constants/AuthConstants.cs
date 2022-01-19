using System;

namespace Spirebyte.Services.Identity.Core.Constants;

public static class AuthConstants
{
    public static int LoginFailuresBeforeLockout = 3;
    public static TimeSpan DefaultLockoutTime = new(3, 0, 0);
}