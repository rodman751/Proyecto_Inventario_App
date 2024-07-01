using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.Entidades.DTO
{
    public class DetalleAjusteProductoDTO
    {
        public DateTime Fecha { get; set; }
        public int ID_DetalleAjuste { get; set; }
        public int ID_Producto { get; set; }
        public string NombreProducto { get; set; }
        public string CodigoProducto { get; set; }
        public int CantidadAjustada { get; set; }
        public string RazonAjuste { get; set; }
    }
}
