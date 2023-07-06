using Microsoft.Data.SqlClient;
using System.Data;
using AppAempaPasteleria.Models;

namespace AppAempaPasteleria.Modulos
{
    public class categoriaDAO
    {
        private conexionDAO cn = new conexionDAO();

        public IEnumerable<Categoria> listado()
        {
            List<Categoria> temporal = new List<Categoria>();
            try
            {
                cn.getcn.Open();
                SqlCommand cmd = new SqlCommand("usp_listarCate", cn.getcn);
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    temporal.Add(new Categoria()
                    {
                        idCat = rd.GetString(0),
                        nomCat = rd.GetString(1)
                    });
                }
                rd.Close();
            }
            catch(SqlException ex) { temporal = new List<Categoria>(); }
            finally { cn.getcn.Close(); }
            return temporal;
        }
    }
}
