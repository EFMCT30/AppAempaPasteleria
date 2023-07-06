using Microsoft.Data.SqlClient;
using System.Data;
using AppAempaPasteleria.Models;

namespace AppAempaPasteleria.Modulos
{
    public class paisDAO
    {
        private conexionDAO cn = new conexionDAO();

        public IEnumerable<Pais> listado()
        {
            List<Pais> temporal = new List<Pais>();
            try
            {
                cn.getcn.Open();
                SqlCommand cmd = new SqlCommand("usp_listarPais", cn.getcn);
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    temporal.Add(new Pais()
                    {
                        idpais = rd.GetString(0),
                        nombrepais = rd.GetString(1)
                    });
                }
                rd.Close();
            }
            catch (SqlException ex) { temporal = new List<Pais>(); }
            finally { cn.getcn.Close(); }
            return temporal;
        }
    }
}
