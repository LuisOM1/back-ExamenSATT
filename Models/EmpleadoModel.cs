using System.ComponentModel.DataAnnotations.Schema;

namespace ExamenSATT.Models
{
    public class EmpleadoModel
    {
        public int id_empl { get; set; }
        public int? id_area { get; set; }
        public string? nomb_empl { get; set; }
        public string? ape_empl { get; set; }
        public string? emai_empl { get; set; }
        public decimal suel_empl { get; set; }
        public DateTime? fech_ingr { get; set; }
        public string? esta { get; set; }
        [NotMapped] // Le indica a EF que no busque esa columna en la tabla física, pero la usara para el SP
        public string? name_area { get; set; }
    }
}
