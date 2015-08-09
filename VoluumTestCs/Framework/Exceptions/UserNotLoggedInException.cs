using System;

namespace VoluumTestCs.Framework.Exceptions
{
    public class UserNotLoggedInException : Exception
    {
        public UserNotLoggedInException() : base("User is not logged in") { }
    }
}
