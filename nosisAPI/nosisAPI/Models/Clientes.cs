using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nosisAPI.Models
{
    [Table("Clientes")]
    public class Clientes
    {
        [Key]
        [Required]
        public long? DNI { get; set; }

        [MaxLength(100) ]
        public string Apellido { get; set; }

        [MaxLength(100)]
        public string Nombre { get; set; }

        [MaxLength(1)]
        public string Genero { get; set; }

        public DateTime FechaNacimiento { get; set; }

        [MaxLength(20)]
        public string Telefono { get; set; }

        [MaxLength(20)]
        public string Celular { get; set; }

        [MaxLength(100)]
        public string Direccion { get; set; }

        [MaxLength(10)]
        public string Postal { get; set; }

        public int? Provincia { get; set; }

        [MaxLength(100)]
        public string ProvinciaNombre { get; set; }

        public int? Ciudad { get; set; }

        [MaxLength(100)]
        public string CiudadNombre { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        public int? Grupo { get; set; }

        [MaxLength(20)]
        public string Tarjeta { get; set; }

        public System.Nullable<DateTime> FechaAlta { get; set; }

        public System.Nullable<DateTime> FechaModif { get; set; }

        public int? Enviado { get; set; }

        [MaxLength(100)]
        public string HashValidacion { get; set; }

        public bool Validado { get; set; }

    }
}
