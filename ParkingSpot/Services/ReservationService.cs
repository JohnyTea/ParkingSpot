using MyParkingSpot.Api.Model;
using MyParkingSpot.Api.Services.Exceptions;
using System.Globalization;

namespace MyParkingSpot.Api.Services;

public class ParkingSpotReservationService
{
    public static int _id;
    public static List<Reservation> _reservations = new();
    public static List<ParkingSpot> _parkingSpotNames = new()
    {
        new ParkingSpot(new Guid(), "01"),
        new ParkingSpot(new Guid(), "02"),
        new ParkingSpot(new Guid(), "03")
    };

    public List<Reservation> GetAllForCurrentWeek()
    {
        var weekNumber = ISOWeek.GetWeekOfYear(DateTime.UtcNow);
        var weeksThisYear = ISOWeek.GetWeeksInYear(DateTime.UtcNow.Year);
        var nextWeekNumber = weekNumber != weeksThisYear ? weekNumber + 1 : 1;
        return _reservations.Where(x => ISOWeek.GetWeekOfYear(x.Date) == weekNumber || ISOWeek.GetWeekOfYear(x.Date) == nextWeekNumber && x.Date >= DateTime.UtcNow ).ToList();
    }

    public Reservation Get(Guid id) => _reservations.SingleOrDefault(r => r.Id == id);

    public Guid Add(Reservation reservation)
    {
        if (reservation.ReservableObject == null)
        {
            throw new ReservationObjectNotExistsException("Reservation object is null");
        }

        if (reservation.ReservableObject.GetType() != typeof(ParkingSpot)){
            throw new ArgumentTypeException("You can reserve only parking spots");
        }

        if (reservation.Date < DateTime.UtcNow)
        {
            throw new ArgumentException("Date can't be from past");
        }

        if (!IsDateInCurrentWeek(reservation.Date))
        {
            throw new ArgumentException("Date must be from current week");
        }

        ParkingSpot parkingSpotToReserve = reservation.ReservableObject as ParkingSpot;

        if (_parkingSpotNames.All(p => parkingSpotToReserve.Code != p.Code))
        {
            throw new ReservationObjectNotExistsException($"Parking spot with code {parkingSpotToReserve.Code} dosn't exist");
        }

        bool parkingSpotIsAlreadyReserved = _reservations.Any(r => r.ReservableObject.Equals(parkingSpotToReserve) &&
            r.Date.Date == reservation.Date.Date);

        if (parkingSpotIsAlreadyReserved)
        {
            throw new ObjectAlreadyReservedException("This parking spot is already reserved");
        }

        reservation.Id = Guid.NewGuid();
        _reservations.Add(reservation);
        return reservation.Id;
    }

    public Guid Update(Reservation updatedReservation)
    {
        var existingReservation = _reservations.SingleOrDefault(r => r.Id == updatedReservation.Id);
        if (existingReservation is null)
        {
            throw new ReservationObjectNotExistsException($"Reservation with id {updatedReservation.Id} not found");
        }

        if (updatedReservation.ReservableObject == null)
        {
            throw new ReservationObjectNotExistsException("Object to reserve is null");
        }

        if (updatedReservation.ReservableObject.GetType() != typeof(ParkingSpot))
        {
            throw new ArgumentTypeException("You can reserve only parking spots");
        }

        if (updatedReservation.Date < DateTime.UtcNow)
        {
            throw new ArgumentException("Date can't be from past");
        }

        bool parkingSpotIsAlreadyReserved = _reservations.Any(r => r.ReservableObject.Equals(updatedReservation.ReservableObject) &&
            r.Date.Date == updatedReservation.Date.Date);

        if (parkingSpotIsAlreadyReserved)
        {
            throw new ObjectAlreadyReservedException("This parking spot is already reserved");
        }

        existingReservation.ReservableObject = updatedReservation.ReservableObject;

        return existingReservation.Id;
    }

    public void Delete(Guid id)
    {
        var reservation = _reservations.SingleOrDefault(r => r.Id == id);
        _reservations.Remove(reservation);
    }

    private static bool IsDateInCurrentWeek(DateTime date)
    {
        var weekToCheck = ISOWeek.GetWeekOfYear(date);
        var currentWeek = ISOWeek.GetWeekOfYear(DateTime.Today);
        return weekToCheck == currentWeek;
    }
}

