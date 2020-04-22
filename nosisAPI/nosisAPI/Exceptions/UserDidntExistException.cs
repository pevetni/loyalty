using System;
namespace nosisAPI.Exceptions
{
    public class UserDidntExistException : Exception
    {
        public UserDidntExistException()
        {
        }

        public UserDidntExistException(string message) : base(message)
        {
        }

        public UserDidntExistException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
