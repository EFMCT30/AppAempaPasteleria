using System.ComponentModel.DataAnnotations;
namespace AppAempaPasteleria.Models
{
    public class Cliente
    {
        [Display (Name = "Código")]
        public string codCli { get; set; }

        [Display(Name = "Nombre"), Required]
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        [RegularExpression(pattern: "[a-zA-Z ]+",
           ErrorMessage = "Solo ingrese letras y espacio en blanco")]
        public string nomCli { get; set; }

       [Display(Name = "Apellido"), Required]
        [StringLength(maximumLength: 50, MinimumLength = 5)]
       [RegularExpression(pattern: "[a-zA-Z ]+",
           ErrorMessage = "Solo ingrese letras y espacio en blanco")]
        public string apeCli { get; set; }

        [Display(Name = "Dni"), Required]
        [StringLength(maximumLength: 10, MinimumLength = 5)]
        public string dniCli { get; set; }

        [Display(Name = "Dirección"), Required]
        [StringLength(maximumLength: 50, MinimumLength = 10)]
        public string dirCli { get; set; }

        [Display(Name = "Pais")]
        public string idPais { get; set; }

        [Display(Name = "Pais")]
        public string idPaisD { get; set; }

        [Display(Name = "Correo"), Required]
       [DataType(DataType.EmailAddress)]
        public string correoCli { get; set; }

        [Display(Name = "Telefono"), Required]
        [RegularExpression(pattern: "[2-5, 9][0-9]{8}")]
        public string telCli { get; set; }

        public int totalPedidos { get; set; }

        public Cliente()
        {
            codCli = "";
            nomCli = "";
            apeCli = "";
            dniCli = "";
            dirCli = "";
            idPais = "";
            idPaisD = "";
            correoCli = "";
            telCli = "";
            totalPedidos = 0;
        }

    }
}
