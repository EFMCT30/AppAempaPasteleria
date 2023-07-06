using System.ComponentModel.DataAnnotations;
namespace AppAempaPasteleria.Models
{
    public class Proveedor
    {
        [Display(Name = "Código")]
        public string idProv { get; set; }

        [Display(Name = "Raz. Social"), Required]
        [StringLength(maximumLength: 35, MinimumLength = 5)]
        [RegularExpression(pattern: "[a-zA-Z ]+",
           ErrorMessage = "Solo ingrese letras y espacio en blanco")]
        public string razProv { get; set; }

        [Display(Name = "Ruc"), Required]
        public string rucProv { get; set; }

        [Display(Name = "Telefono"), Required]
        [RegularExpression(pattern: "[2-5, 9][0-9]{8}")]
        public string telProv { get; set; }

        [Display(Name = "Correo"), Required]
        [DataType(DataType.EmailAddress)]
        public string correoProv { get; set; }

        [Display(Name = "Dirección"), Required]
        [StringLength(maximumLength: 50, MinimumLength = 10)]
        public string direcProv { get; set; }

        [Display(Name = "Pais")]
        public string idPais { get; set; }

        [Display(Name = "Pais")]
        public string idPaisD { get; set; }

        public Proveedor()
        {
            idProv = "";
            razProv = "";
            rucProv = "";
            telProv = "";
            correoProv = "";
            direcProv = "";
            idPais = "";
            idPaisD = "";
        }
    }
}
