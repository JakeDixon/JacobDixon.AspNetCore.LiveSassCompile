using System;
using System.Collections.Generic;
using System.Text;

namespace JacobDixon.AspNetCore.LiveSassCompile.Exceptions
{

    [Serializable]
    public class EmptyStringException : Exception
    {
        public EmptyStringException() { }
        public EmptyStringException(string message) : base(message) { }
        public EmptyStringException(string message, Exception inner) : base(message, inner) { }
        protected EmptyStringException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
