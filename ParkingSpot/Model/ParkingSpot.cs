using MyParkingSpot.Api.Services;

namespace MyParkingSpot.Api.Model
{
    public class ParkingSpot : IReservable
    {
        public Guid Id { get; }
        public string Code { get; }

        public ParkingSpot(Guid id, string code)
        {
            Id = id;
            Code = code;
        }

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
