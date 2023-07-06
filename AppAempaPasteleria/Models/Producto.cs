using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AppAempaPasteleria.Models
{
    public class Producto
    {
        [Display(Name = "Código")]
        public string idProd { get; set; }

        [Display(Name = "Nombre"), Required]
        public string nomProd { get; set; }

        [Display(Name = "Imagen")]
        public string fotoProd { get; set; }

        [Display(Name = "Insumos"), Required]
        public string idIn { get; set; }

        [Display(Name = "Categoria")]
        public string idCate { get; set; }
        [Display(Name = "Categoria")]
        public string idCateD { get; set; }

        [Display(Name = "Descripción"), Required]
        public string descProd { get; set; }

        [Display(Name = "Precio"), Required]
        public decimal preProd { get; set; }

        [Display(Name = "Stock"), Required]
        public int stockProd { get; set; }

        public string fotoProdPath { get; set; }

        public Producto()
        {
            idProd = "";
            nomProd = "";
            fotoProd = "";
            idIn = "";
            idCate = "";
            idCateD = "";
            descProd = "";
            preProd = 0;
            stockProd = 0;
            fotoProdPath = "";
        }
    }
}
