using Microsoft.AspNetCore.Mvc;
using MyParkingSpot.Api.Controllers.Commands;
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
    public ActionResult<List<ParkingSpotReservationDTO>> Get() {

        List<Reservation> reservations = reservationService.GetAllForCurrentWeek();
        List<ParkingSpotReservationDTO> result= new();

        foreach (Reservation reservation in reservations)
        {
            ParkingSpot parkingSpot = reservation.ReservableObject as ParkingSpot;
            ParkingSpotReservationDTO resultParkingSpot = new ParkingSpotReservationDTO(parkingSpot.Code, reservation.Owner.Id, reservation.Date, reservation.Id);
            result.Add(resultParkingSpot);
        }

        return Ok(result);
    }
    
    [HttpGet("{id:guid}")]
    public ActionResult<ParkingSpotReservationDTO> Get(Guid id)
    {
        var reservation = reservationService.Get(id);
        if (reservation is null)
        {
            return NotFound();
        }
        ParkingSpot parkingSpot = reservation.ReservableObject as ParkingSpot;
        ParkingSpotReservationDTO result = new ParkingSpotReservationDTO(parkingSpot.Code, reservation.Owner.Id, reservation.Date, reservation.Id);

        return Ok(result);
    }

    [HttpPost]
    public ActionResult Post(CreateReservation command)
    {
        try
        {
            Guid reservationID = reservationService.Add(command);
            return CreatedAtAction(nameof(Get), new { id = reservationID }, null);
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

        return BadRequest();
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(ChangeParkingSpot command)
    {
        try
        {
            reservationService.Update(command);
        }
        catch(ReservationObjectNotExistsException e)
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

    [HttpDelete("{id:guid}")]
    public ActionResult Delete(Guid id)
    {
        reservationService.Delete(id);

        return NoContent();
    }

}

