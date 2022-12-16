namespace MyParkingSpot.Api.Model
{
    public class User : IReservationOwner
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LicencePlate { get; set; }

        public User()
        {
            Id = Guid.NewGuid();
        }
    }
}
