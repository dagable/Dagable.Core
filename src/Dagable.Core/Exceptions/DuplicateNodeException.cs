using Dagable.Core.Models;
using System;

namespace Dagable.Core.Exceptions
{
    internal class DuplicateNodeException : Exception
    {
        public const string INVALID_STANDARD_NODE = "Error in StandardNode with Id: {0}";

        public DuplicateNodeException(string message) : base(message) { }

    }
}
