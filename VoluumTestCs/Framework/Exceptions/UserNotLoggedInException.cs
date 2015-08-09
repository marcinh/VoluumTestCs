using System;

namespace VoluumTestCs.Framework.Exceptions
{
    /// <summary>
    /// Thrown when login was unsuccesfull
    /// </summary>
    public class UserNotLoggedInException : Exception
    {
        public UserNotLoggedInException() : base("User is not logged in") { }
    }
}
