namespace ExamenSATT.Models
{
    public class AreaModel
    {
        public int id_area { get; set; }
        public string? name_area { get; set; }

    }

    // El modelo debe representar exactamente igual a los campos y nombres de la tabla de la BD
    // ? significa que si puede ser nulo.

    // Usando entity framework(se descarga de nudget) se crea automaticamente el
    // codigo y el archivo del modelo ejecutando 1 solo comando

    //Tambine existe proyecto aplicacion web modelo vista controldor con .NET 7 pero ahi
    //tiene vista, es 1 monolito backend con vista en .NET
}
