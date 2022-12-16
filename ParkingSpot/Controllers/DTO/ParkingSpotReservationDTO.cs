namespace MyParkingSpot.Api.Controllers.DTO
{
    public class ParkingSpotReservationDTO
    {
        public string ParkingSpotCode { get; set; }
        public Guid OwnerID { get; set; }
        public DateTime DateOfReservation { get; set; }

        public ParkingSpotReservationDTO(string parkingSpotCode, Guid ownerID, DateTime dateOfReservation)
        {
            ParkingSpotCode = parkingSpotCode;
            OwnerID = ownerID;
            DateOfReservation = dateOfReservation;
        }

    }
}
