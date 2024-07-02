using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario.Entidades
{
    public class AjusteProducto
    {
        [Key]
        public int ID_Ajuste { get; set; }

        [Required]
        [MaxLength(50)]
        public string NumeroAjuste { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        public string Descripcion { get; set; }

        [Required]
        public bool Impreso { get; set; }


        public List<DetalleAjusteProducto>? DetalleAjusteProducto { get; set; }
    }
}



