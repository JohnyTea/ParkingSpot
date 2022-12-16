using MyParkingSpot.Api.Services;

namespace MyParkingSpot.Api.Model;

public class Reservation
{
    public Guid Id { get; init; }
    public DateTime Date { get; private set; }
    public IReservable ReservableObject { get; set; }
    public IReservationOwner Owner { get; private set; }
    public Reservation(IReservationOwner owner, IReservable reservableObject, DateTime date)
    {
        Id = new Guid();
        Owner = owner;
        ReservableObject = reservableObject;
        Date = date;
    }
}

