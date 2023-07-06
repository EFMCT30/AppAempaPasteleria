using System.ComponentModel.DataAnnotations;
namespace AppAempaPasteleria.Models
{
    public class Insumo
    {
        [Display(Name = "Código")]
        public string idIn { get; set; }

        [Display(Name = "Descripción"), Required]
        public string desIn { get; set; }

        [Display(Name = "Proveedor")]
        public string idProv { get; set; }

        [Display(Name = "Proveedor")]
        public string idProvD { get; set; }

        [Display(Name = "Fec. Compra"), Required]
        public DateTime? fecComIn { get; set; }

        [Display(Name = "Fec. Vencimiento"), Required]
        public DateTime? fecVenIn { get; set; }

        public Insumo() 
        {
            idIn = "";
            desIn = "";
            idProv = "";
            idProvD = "";
            fecComIn = null;
            fecVenIn=null;
        }
    }
}
