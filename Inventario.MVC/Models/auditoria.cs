namespace Inventario.MVC.Models
{
    public class auditoria
    {
        public string aud_usuario { get; set; }
        public string aud_accion { get; set; } 
        public string aud_modulo { get; set; } 
        public string aud_funcionalidad { get; set; }
        public string aud_observacion { get; set; } = "";
    }
}
