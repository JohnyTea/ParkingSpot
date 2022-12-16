using MyParkingSpot.Api.Controllers.Commands;
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
        new ParkingSpot("01"),
        new ParkingSpot("02"),
        new ParkingSpot("03")
    };

    public List<Reservation> GetAllForCurrentWeek()
    {
        var weekNumber = ISOWeek.GetWeekOfYear(DateTime.UtcNow);
        var weeksThisYear = ISOWeek.GetWeeksInYear(DateTime.UtcNow.Year);
        var nextWeekNumber = weekNumber != weeksThisYear ? weekNumber + 1 : 1;
        return _reservations.Where(x => ISOWeek.GetWeekOfYear(x.Date) == weekNumber || ISOWeek.GetWeekOfYear(x.Date) == nextWeekNumber && x.Date >= DateTime.UtcNow ).ToList();
    }

    public Reservation Get(Guid id) => _reservations.SingleOrDefault(r => r.Id == id);

    public Guid Add(CreateReservation command)
    {
        if (command.ParkingSpotCode is null)
        {
            throw new ReservationObjectNotExistsException("Reservation object is null");
        }

        ParkingSpot parkingSpotToReserve = _parkingSpotNames.SingleOrDefault(p => p.Code == command.ParkingSpotCode);

        if (parkingSpotToReserve is null)
        {
            throw new ReservationObjectNotExistsException($"Parking spot with code {command.ParkingSpotCode} dosn't exist");
        }

        if (command.DateOfReservation < DateTime.UtcNow)
        {
            throw new ArgumentException("Date can't be from past");
        }

        if (!IsDateInCurrentWeek(command.DateOfReservation))
        {
            throw new ArgumentException("Date must be from current week");
        }

        bool parkingSpotIsAlreadyReserved = _reservations.Any(r => r.ReservableObject.Equals(parkingSpotToReserve) &&
            r.Date == command.DateOfReservation);

        if (parkingSpotIsAlreadyReserved)
        {
            throw new ObjectAlreadyReservedException("This parking spot is already reserved");
        }

        User user = new User(); // TODO add real users
        Reservation newReservation = new Reservation(user, parkingSpotToReserve, command.DateOfReservation);
        
        _reservations.Add(newReservation);
        return newReservation.Id;
    }

    public Guid Update(ChangeParkingSpot command)
    {
        var existingReservation = _reservations.SingleOrDefault(r => r.Id == command.ReservationID);
        if (existingReservation is null)
        {
            throw new ReservationObjectNotExistsException($"Reservation with id {command.ReservationID} not found");
        }

        if (command.ParkingSpotCode == null)
        {
            throw new ReservationObjectNotExistsException("Object to reserve is null");
        }

        ParkingSpot parkingSpotToReserve = _parkingSpotNames.SingleOrDefault(p => p.Code == command.ParkingSpotCode);

        bool parkingSpotIsAlreadyReserved = _reservations.Any(r => r.ReservableObject.Equals(parkingSpotToReserve) &&
            r.Date == existingReservation.Date);

        if (parkingSpotIsAlreadyReserved)
        {
            throw new ObjectAlreadyReservedException("This parking spot is already reserved");
        }

        existingReservation.ReservableObject = parkingSpotToReserve;

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

