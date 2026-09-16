namespace Order.Aplicacao.Handles.Reservation;

public class CreateReservationCommandResponse
{
    public bool IsCreated { get; set; }
    public DateTime ReservationDate { get; set; }
}
