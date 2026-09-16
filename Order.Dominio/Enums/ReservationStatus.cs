namespace Order.Dominio.Enums
{
    public enum ReservationStatus
    {
        [EnumText("Ativo")]
        Active = 1,

        [EnumText("Cancelada")]
        Cancelled = 2,

        [EnumText("Expirado")]
        Expired = 3
    }
}
