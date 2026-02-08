namespace ExamenSATT.DTOs
{
    // El DTO no es igual al Modelo, solo son los datos que se quiere obtener y exponer al frontend
    public class EmpleadoDTO
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public decimal Sueldo { get; set; }
        public string? AreaNombre { get; set; }
        public string? Estado { get; set; }
    }
}
