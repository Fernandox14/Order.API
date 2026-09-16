namespace Order.Dominio.Enums
{
    public enum ProductStatus
    {
        [EnumText("Disponível")]
        Available = 1,

        [EnumText("Reservado")]
        Reserved = 2,

        [EnumText("Indisponível")]
        Unavailable = 3
    }
}