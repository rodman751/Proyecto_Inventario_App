using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inventario.Entidades
{
    public class MovimientoInventario
    {
        [Key]
        public int ID_Movimiento { get; set; }

        [ForeignKey("Producto")]
        public int ID_Producto { get; set; }

        [Required]
        [MaxLength(50)]
        public string TipoMovimiento { get; set; }

        [Required]
        [MaxLength(50)]
        public string NumeroDocumento { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaMovimiento { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public int StockAnterior { get; set; }

        [Required]
        public int StockActual { get; set; }

        public Producto Producto { get; set; }
    }
}
