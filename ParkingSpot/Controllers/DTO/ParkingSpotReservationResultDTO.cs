namespace MyParkingSpot.Api.Controllers.DTO
{
    public class ParkingSpotReservationResultDTO
    {
        public Guid ReservationID { get; set; }
        public string ParkingSpotCode { get; set; }
        public Guid OwnerID { get; set; }
        public DateTime DateOfReservation { get; set; }

        public ParkingSpotReservationResultDTO(string parkingSpotCode, Guid ownerID, DateTime dateOfReservation, Guid reservationID)
        {
            ParkingSpotCode = parkingSpotCode;
            OwnerID = ownerID;
            DateOfReservation = dateOfReservation;
            ReservationID = reservationID;
        }
    }
}
