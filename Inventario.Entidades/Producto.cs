using System.ComponentModel.DataAnnotations;

namespace Inventario.Entidades
{
    public class Producto
    {
        [Key]
        public int ID_Producto { get; set; }

        [Required]
        [MaxLength(50)]
        public string Codigo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        [Required]
        public bool GravaIVA { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Costo { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal PVP { get; set; }

        [Required]
        public bool Estado { get; set; }

        public int StockProducto { get; set; }


    }
}
