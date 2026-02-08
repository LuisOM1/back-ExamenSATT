namespace ExamenSATT.DTOs
{
    public class EmpleadoCreateUpdateDTO
    {
        public int IdArea { get; set; }
        public string NombEmpl { get; set; } = null!;
        public string ApeEmpl { get; set; } = null!;
        public string? EmaiEmpl { get; set; }
        public decimal SuelEmpl { get; set; }
        public DateTime FechIngr { get; set; }

    }
    // Para implementar estas operaciones manteniendo el nivel Pro con .NET 8, EF Core y AutoMapper,
    // debemos seguir el flujo de capas que ya tienes: DTO -> Repository -> Service -> Controller.
    
    // Este DTO es el DTO de Entrada (Create/Update)
    // Es una buena práctica separar el DTO de lectura del de creación, para no exponer el ID cuando
    // no es necesario
}
