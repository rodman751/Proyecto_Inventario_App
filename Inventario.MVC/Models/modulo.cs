using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario.MVC.Models
{
    public class modulo
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string[] Role { get; set; }
        public string[] functionalities { get; set; }
    }
}
