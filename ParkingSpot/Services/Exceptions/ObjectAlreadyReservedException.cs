using System.Runtime.Serialization;

namespace MyParkingSpot.Api.Services.Exceptions
{
    [Serializable]
    internal class ObjectAlreadyReservedException : Exception
    {
        public ObjectAlreadyReservedException()
        {
        }

        public ObjectAlreadyReservedException(string? message) : base(message)
        {
        }

        public ObjectAlreadyReservedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ObjectAlreadyReservedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}