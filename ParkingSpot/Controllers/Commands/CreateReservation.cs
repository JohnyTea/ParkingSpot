namespace MyParkingSpot.Api.Controllers.Commands;

public record CreateReservation(Guid OwnerID, DateTime DateOfReservation, string ParkingSpotCode);
