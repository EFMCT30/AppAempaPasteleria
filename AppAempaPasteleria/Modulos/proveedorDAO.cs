using AppAempaPasteleria.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AppAempaPasteleria.Modulos
{
    public class proveedorDAO
    {
        private conexionDAO cn = new conexionDAO();

        //listar
        public IEnumerable<Proveedor> listado()
        {
            List<Proveedor> temporal = new List<Proveedor>();
            try
            {
                cn.getcn.Open();
                SqlCommand cmd = new SqlCommand("usp_listarProve",cn.getcn);
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    temporal.Add(new Proveedor()
                    {
                        idProv=rd.GetString(0),
                        razProv=rd.GetString(1),
                        rucProv=rd.GetString(2),
                        telProv=rd.GetString(3),
                        correoProv=rd.GetString(4),
                        direcProv=rd.GetString(5),
                        idPais=rd.GetString(6),
                        idPaisD = rd.GetString(7),
                    });
                }
                rd.Close();
            }
            catch (SqlException ex) { temporal = new List<Proveedor>(); }
            finally { cn.getcn.Close(); }
            return temporal;
        }

        //insertar
        public string Agregar(Proveedor reg)
        {
            string mensaje = "";
            try
            {
                cn.getcn.Open();
                SqlCommand cmd = new SqlCommand("usp_GuardarProve", cn.getcn);
                cmd.CommandType= CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@razProv", reg.razProv);
                cmd.Parameters.AddWithValue("@rucProv", reg.rucProv);
                cmd.Parameters.AddWithValue("@telProv", reg.telProv);
                cmd.Parameters.AddWithValue("@correoProv", reg.correoProv);
                cmd.Parameters.AddWithValue("@direcProv", reg.direcProv);
                cmd.Parameters.AddWithValue("@idPais", reg.idPais);
                int c = cmd.ExecuteNonQuery();
                mensaje = $"Se ha agregado {c} Proveedor(s) : {reg.razProv}";
            }
            catch (SqlException ex) { mensaje = ex.Message; }
            finally { cn.getcn.Close(); }
            return mensaje;
        }

        //editar
        public string Actualizar(Proveedor reg)
        {
            string mensaje = "";
            try
            {
                cn.getcn.Open();
                SqlCommand cmd = new SqlCommand("usp_EditarProve", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProv", reg.idProv);
                cmd.Parameters.AddWithValue("@razProv", reg.razProv);
                cmd.Parameters.AddWithValue("@rucProv", reg.rucProv);
                cmd.Parameters.AddWithValue("@telProv", reg.telProv);
                cmd.Parameters.AddWithValue("@correoProv", reg.correoProv);
                cmd.Parameters.AddWithValue("@direcProv", reg.direcProv);
                cmd.Parameters.AddWithValue("@idPais", reg.idPais);
                int c = cmd.ExecuteNonQuery();
                mensaje = $"Se ha actualizado {c} Proveedor(s) : {reg.razProv}";
            }
            catch (SqlException ex) { mensaje = ex.Message; }
            finally { cn.getcn.Close(); }
            return mensaje;
        }

        //buscar
        public Proveedor Buscar(string idProv)
        {
            return listado().FirstOrDefault(x => x.idProv == idProv);
        }

        //eliminar
        public string Eliminar(string idProv)
        {
            string mensaje = "";
            try
            {
                cn.getcn.Open();
                SqlCommand cmd = new SqlCommand("usp_EliminarProve", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProv",idProv);
                
                int c = cmd.ExecuteNonQuery();
                mensaje = $"Se ha eliminado {c} Proveedor(s) : "+idProv;
            }
            catch (SqlException ex) { mensaje = ex.Message; }
            finally { cn.getcn.Close(); }
            return mensaje;
        }

    }
}
