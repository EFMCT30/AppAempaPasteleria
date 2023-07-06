using System.ComponentModel.DataAnnotations;

namespace AppAempaPasteleria.Models
{
    public class Pedido
    {
        [Display(Name = "Código"), Required]
        public int idPed { get; set; }

        [Display(Name = "Fecha")]
        public DateTime fechaPed { get; set; }

        [Display(Name = "Cliente"), Required]
        public string? idCli { get; set; }

        [Display(Name = "Método de Pago"), Required]
        public string? metodoPed { get; set; }

        [Display(Name = "Tipo Entrega"), Required]
        public string? EntregaPed { get; set; }

        [Display(Name = "Estado"), Required]
        public string? EstadoPed { get; set; }

        [Display(Name = "Código"), Required]
        public string? IdPro { get; set; }

        [Display(Name = "Producto"), Required]
        public string? NompPro { get; set; }

        [Display(Name = "Cliente"), Required]
        public string? NomCli { get; set; }

        [Display(Name = "Monto"), Required]
        public decimal monto { get; set; }

        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Display(Name = "Precio")]
        public decimal precioU { get; set; }
    }
}
