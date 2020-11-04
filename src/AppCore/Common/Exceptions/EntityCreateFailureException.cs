using System;

namespace AppCore.Common.Exceptions
{
    public class EntityCreateFailureException : Exception
    {
        public EntityCreateFailureException(string name, object key, string message)
            : base($"Creation of entity \"{name}\" ({key}) failed. {message}")
        {
        }
    }
}
