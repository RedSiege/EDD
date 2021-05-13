using System;

namespace EDD.Models
{
    public class EDDException : Exception
    {
        public EDDException() { }

        public EDDException(string message) : base(message) { }

        public EDDException(string message, Exception inner) : base(message, inner) { }
    }
}
