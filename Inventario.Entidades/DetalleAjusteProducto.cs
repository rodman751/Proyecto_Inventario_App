using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inventario.Entidades
{
    public class DetalleAjusteProducto
    {
        [Key]
        public int ID_DetalleAjuste { get; set; }

        [ForeignKey("AjusteProducto")]
        public int ID_Ajuste { get; set; }

        [ForeignKey("Producto")]
        public int ID_Producto { get; set; }

        [Required]
        public int CantidadAjustada { get; set; }

        public string RazonAjuste { get; set; }

        public AjusteProducto ?AjusteProducto { get; set; }
        public Producto ?Producto { get; set; }
    }
}
