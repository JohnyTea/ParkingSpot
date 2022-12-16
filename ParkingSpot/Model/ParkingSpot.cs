using MyParkingSpot.Api.Services;

namespace MyParkingSpot.Api.Model
{
    public class ParkingSpot : IReservable
    {
        public Guid Id { get; init; }
        public string Code { get; private set; }

        public ParkingSpot(string code)
        {
            Id = new Guid();
            Code = code;
        }

        public override bool Equals(object? obj)
        {
            return obj is ParkingSpot spot &&
                   Code == spot.Code;
        }
    }
}
