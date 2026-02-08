namespace ExamenSATT.DTOs
{
    // DTO para la respuesta de la API en swagger
    public class APIResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
