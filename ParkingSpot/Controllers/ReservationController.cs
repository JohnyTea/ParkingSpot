using Microsoft.AspNetCore.Mvc;
using MyParkingSpot.Api.Controllers.DTO;
using MyParkingSpot.Api.Model;
using MyParkingSpot.Api.Services;
using MyParkingSpot.Api.Services.Exceptions;

namespace MyParkingSpot.Api.Controllers;

[ApiController]
[Route("reservations")]
public class ReservationsController : ControllerBase
{
    private readonly ParkingSpotReservationService reservationService = new();

    [HttpGet]
    public ActionResult<List<ParkingSpotReservationResultDTO>> Get() {

        List<Reservation> reservations = reservationService.GetAllForCurrentWeek();
        List<ParkingSpotReservationResultDTO> result= new();

        foreach (Reservation reservation in reservations)
        {
            ParkingSpot parkingSpot = reservation.ReservableObject as ParkingSpot;
            ParkingSpotReservationResultDTO resultParkingSpot = new ParkingSpotReservationResultDTO(parkingSpot.Code, reservation.Owner.Id, reservation.Date, reservation.Id);
            result.Add(resultParkingSpot);
        }

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public ActionResult<ParkingSpotReservationResultDTO> Get(Guid id)
    {
        var reservation = reservationService.Get(id);
        if (reservation is null)
        {
            return NotFound();
        }
        ParkingSpot parkingSpot = reservation.ReservableObject as ParkingSpot;
        ParkingSpotReservationResultDTO result = new ParkingSpotReservationResultDTO(parkingSpot.Code, reservation.Owner.Id, reservation.Date, reservation.Id);

        return Ok(result);
    }

    [HttpPost]
    public ActionResult Post(ParkingSpotReservationDTO reservationDTO)
    {
        User user = new User(reservationDTO.OwnerID);
        ParkingSpot parkingSpot = new ParkingSpot(reservationDTO.ParkingSpotCode);
        Reservation reservation = new Reservation(user, parkingSpot, reservationDTO.DateOfReservation);
        try
        {
            reservationService.Add(reservation);
        }
        catch (ReservationObjectNotExistsException e)
        {
            return NotFound(e.Message);
        }
        catch (ArgumentTypeException e)
        {
            return BadRequest(e.Message);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (ObjectAlreadyReservedException e)
        {
            return BadRequest(e.Message);
        }

        return CreatedAtAction(nameof(Get), new { id = reservation.Id }, null);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(Guid id, ParkingSpotReservationDTO reservationDTO)
    {
        User user = new User(reservationDTO.OwnerID);
        ParkingSpot parkingSpot = new ParkingSpot(reservationDTO.ParkingSpotCode);
        Reservation reservation = new Reservation(user, parkingSpot, reservationDTO.DateOfReservation);
        reservation.Id = id;
        try
        {
            reservationService.Update(reservation);
        }catch(ReservationObjectNotExistsException e)
        {
            return NotFound(e.Message);
        }
        catch (ArgumentTypeException e)
        {
            return BadRequest(e.Message);
        }
        catch (ObjectAlreadyReservedException e)
        {
            return BadRequest(e.Message);
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(Guid id)
    {
        reservationService.Delete(id);

        return NoContent();
    }

}

