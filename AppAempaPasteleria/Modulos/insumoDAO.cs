using AppAempaPasteleria.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AppAempaPasteleria.Modulos
{
    public class insumoDAO
    {
        private conexionDAO cn = new conexionDAO();

        //listar
        public IEnumerable<Insumo> listado()
        {
            List<Insumo> temporal = new List<Insumo>();
            try
            {
                cn.getcn.Open();
                SqlCommand cmd = new SqlCommand("usp_listarInsu", cn.getcn);
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    temporal.Add(new Insumo()
                    {
                        idIn = rd.GetString(0),
                        desIn = rd.GetString(1),
                        idProv = rd.GetString(2),
                        idProvD = rd.GetString(3),
                        fecComIn = rd.GetDateTime(4),
                        fecVenIn = rd.GetDateTime(5),
                    });
                }
                rd.Close();
            }
            catch (SqlException ex) { temporal = new List<Insumo>(); }
            finally { cn.getcn.Close(); }
            return temporal;
        }

        //insertar
        public string Agregar(Insumo reg)
        {
            string mensaje = "";
            try
            {
                cn.getcn.Open();
                SqlCommand cmd = new SqlCommand("usp_GuardarInsu", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@desIn", reg.desIn);
                cmd.Parameters.AddWithValue("@idProv", reg.idProv);
                cmd.Parameters.AddWithValue("@fecComIn", reg.fecComIn);
                cmd.Parameters.AddWithValue("@fecVenIn", reg.fecVenIn);
                int c = cmd.ExecuteNonQuery();
                mensaje = $"Se ha agregado {c} Insumo(s) : {reg.desIn}";
            }
            catch (SqlException ex) { mensaje = ex.Message; }
            finally { cn.getcn.Close(); }
            return mensaje;
        }

        //editar
        public string Actualizar(Insumo reg)
        {
            string mensaje = "";
            try
            {
                cn.getcn.Open();
                SqlCommand cmd = new SqlCommand("usp_EditarInsu", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idIn", reg.idIn);
                cmd.Parameters.AddWithValue("@desIn", reg.desIn);
                cmd.Parameters.AddWithValue("@idProv", reg.idProv);
                cmd.Parameters.AddWithValue("@fecComIn", reg.fecComIn);
                cmd.Parameters.AddWithValue("@fecVenIn", reg.fecVenIn);
                int c = cmd.ExecuteNonQuery();
                mensaje = $"Se ha actualizado {c} Insumo(s) : {reg.desIn}";
            }
            catch (SqlException ex) { mensaje = ex.Message; }
            finally { cn.getcn.Close(); }
            return mensaje;
        }

        public Insumo Buscar(string idIn)
        {
            return listado().FirstOrDefault(x => x.idIn == idIn);
        }

        //eliminar
        public string Eliminar(string  idIn)
        {
            string mensaje = "";
            try
            {
                cn.getcn.Open();
                SqlCommand cmd = new SqlCommand("usp_EliminarInsu", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idIn", idIn);
                int c = cmd.ExecuteNonQuery();
                mensaje = $"Se ha actualizado {c} Insumo(s)";
            }
            catch (SqlException ex) { mensaje = ex.Message; }
            finally { cn.getcn.Close(); }
            return mensaje;
        }

    }
}
