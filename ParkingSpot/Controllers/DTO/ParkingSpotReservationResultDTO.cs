namespace MyParkingSpot.Api.Controllers.DTO
{
    public class ParkingSpotReservationDTO
    {
        public Guid ReservationID { get; set; }
        public string ParkingSpotCode { get; set; }
        public Guid OwnerID { get; set; }
        public DateTime DateOfReservation { get; set; }

        public ParkingSpotReservationDTO(string parkingSpotCode, Guid ownerID, DateTime dateOfReservation, Guid reservationID)
        {
            ParkingSpotCode = parkingSpotCode;
            OwnerID = ownerID;
            DateOfReservation = dateOfReservation;
            ReservationID = reservationID;
        }
    }
}
