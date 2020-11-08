using System;
using System.Collections.Generic;
using System.Text;

namespace JacobDixon.AspNetCore.LiveSassCompile.Exceptions
{

    [Serializable]
    public class EmptyArrayException : Exception
    {
        public EmptyArrayException() { }
        public EmptyArrayException(string message) : base(message) { }
        public EmptyArrayException(string message, Exception inner) : base(message, inner) { }
        protected EmptyArrayException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
