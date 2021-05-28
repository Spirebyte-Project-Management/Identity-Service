using System;
using System.Collections.Generic;
using System.Text;

namespace Spirebyte.Services.Identity.Core.Constants
{
    public static class AuthConstants
    {
        public static int LoginFailuresBeforeLockout = 3;
        public static TimeSpan DefaultLockoutTime = new TimeSpan(3, 0,0);
    }
}
