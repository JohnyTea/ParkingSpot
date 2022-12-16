using System.Runtime.Serialization;

namespace MyParkingSpot.Api.Services.Exceptions
{
    [Serializable]
    internal class ArgumentTypeException : Exception
    {
        public ArgumentTypeException()
        {
        }

        public ArgumentTypeException(string? message) : base(message)
        {
        }

        public ArgumentTypeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ArgumentTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}