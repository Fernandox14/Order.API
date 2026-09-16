namespace Order.Aplicacao.Handles.Reservation;

public class DeleteReservationCommandResponse
{
    public bool IsDeleted { get; set; }
    public DateTime ReservationDate { get; set; }
}
