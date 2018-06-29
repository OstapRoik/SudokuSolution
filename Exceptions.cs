using System;

namespace MyProjects.Game
{
    class FormatFieldException : Exception
    {
        public FormatFieldException(string message) : base(message) { }
    }
    class ErrorInFieldException : Exception
    {
        public ErrorInFieldException(string message) : base(message) { }
    }
}