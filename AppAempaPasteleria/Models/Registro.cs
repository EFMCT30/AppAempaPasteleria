using System.ComponentModel.DataAnnotations;

namespace AppAempaPasteleria.Models
{
    public class Registro
    {
        [Display(Name = "Código")]
        public string idProd { get; set; }


        [Display(Name = "Nombre")]
        public string nomProd { get; set; }


        [Display(Name = "Imagen")]
        public string fotoProd { get; set; }


        [Display(Name = "Insumo")]
        public string idIn { get; set; }


        [Display(Name = "Categoria")]
        public string idCate { get; set; }


        [Display(Name = "Descripción")]
        public string descProd { get; set; }


        [Display(Name = "Precio")]
        public decimal preProd { get; set; }


        [Display(Name = "Stock")]
        public int stockProd { get; set; }


        [Display(Name = "Monto")]
        public decimal monto { get { return preProd * stockProd; } }

        public Registro()
        {
            idProd = "";
            nomProd = "";
            fotoProd = "";
            idIn = "";
            idCate = "";
            descProd = "";
            preProd = 0;
            stockProd = 0;
        }
    }
}
