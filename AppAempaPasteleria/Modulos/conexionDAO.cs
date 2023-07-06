using Microsoft.Data.SqlClient;

namespace AppAempaPasteleria.Modulos
{
    public class conexionDAO
    {
        //atributo de conexion de alcance local
        private SqlConnection cn = new SqlConnection(@"server=DESKTOP-HHJH2A8\SQLEXPRESS;Database=Aempap2023;Trusted_Connection=true;MultipleActiveResultSets=True;TrustServerCertificate=false;Encrypt=false;");

        //propiedad donde retorna la conexion
        public SqlConnection getcn { get { return cn; } }
    }
}
