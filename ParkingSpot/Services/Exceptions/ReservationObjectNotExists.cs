using System.Runtime.Serialization;

namespace MyParkingSpot.Api.Services.Exceptions
{
    [Serializable]
    internal class ReservationObjectNotExistsException : Exception
    {
        public ReservationObjectNotExistsException()
        {
        }

        public ReservationObjectNotExistsException(string? message) : base(message)
        {
        }

        public ReservationObjectNotExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ReservationObjectNotExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}