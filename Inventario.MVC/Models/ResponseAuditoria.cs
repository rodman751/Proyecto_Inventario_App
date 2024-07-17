namespace Inventario.MVC.Models
{
    public class ResponseAuditoria
    {
        public string Message { get; set; }
        public Auditoria Auditoria { get; set; }
    }

    public class Auditoria
    {
        public int Aud_id { get; set; }
        public string Aud_usuario { get; set; }
        public DateTime Aud_fecha { get; set; }
        public string Aud_accion { get; set; }
        public string Aud_modulo { get; set; }
        public string Aud_funcionalidad { get; set; }
        public string Aud_observacion { get; set; }
    }
}
