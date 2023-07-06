using AppAempaPasteleria.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AppAempaPasteleria.Modulos
{
    public class clienteDAO
    {
        private conexionDAO cn = new conexionDAO();
        
        //listar
        public IEnumerable<Cliente> listado()
        {
            List<Cliente> temporal = new List<Cliente>();
            try
            {
                cn.getcn.Open(); ;
                SqlCommand cmd = new SqlCommand("usp_listarClientes", cn.getcn);
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    temporal.Add(new Cliente()
                    {
                        codCli=rd.GetString(0),
                        nomCli=rd.GetString(1),
                        apeCli=rd.GetString(2),
                        dniCli=rd.GetString(3),
                        dirCli=rd.GetString(4),
                        idPais=rd.GetString(5),
                        idPaisD = rd.GetString(6),
                        correoCli =rd.GetString(7),
                        telCli=rd.GetString(8),
                    });
                }
                rd.Close();
            }
            catch (SqlException ex) { temporal = new List<Cliente>(); }
            finally { cn.getcn.Close(); }
            return temporal;
        }
        
        //insertar
        public string Agregar(Cliente reg)
        {
            string mensaje = "";
            try
            {
                cn.getcn.Open();
                SqlCommand cmd = new SqlCommand("usp_GuardarCliente", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nomCli", reg.nomCli);
                cmd.Parameters.AddWithValue("@apeCli", reg.apeCli);
                cmd.Parameters.AddWithValue("@dniCli", reg.dniCli);
                cmd.Parameters.AddWithValue("@DireccCli", reg.dirCli);
                cmd.Parameters.AddWithValue("@idPais", reg.idPais);
                cmd.Parameters.AddWithValue("@correoCli", reg.correoCli);
                cmd.Parameters.AddWithValue("@TelefCli", reg.telCli);
                int c = cmd.ExecuteNonQuery();
                mensaje = $"Se ha agregado {c} cliente(s) : {reg.nomCli}";
            }
            catch (SqlException ex) { mensaje = ex.Message; }
            finally { cn.getcn.Close(); }
            return mensaje;
        }

        //editar
        public string Actualizar(Cliente reg)
        {
            string mensaje = "";
            try
            {
                cn.getcn.Open();
                SqlCommand cmd = new SqlCommand("usp_EditarCliente", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCli", reg.codCli);
                cmd.Parameters.AddWithValue("@nomCli", reg.nomCli);
                cmd.Parameters.AddWithValue("@apeCli", reg.apeCli);
                cmd.Parameters.AddWithValue("@dniCli", reg.dniCli);
                cmd.Parameters.AddWithValue("@DireccCli", reg.dirCli);
                cmd.Parameters.AddWithValue("@idPais", reg.idPais);
                cmd.Parameters.AddWithValue("@correoCli", reg.correoCli);
                cmd.Parameters.AddWithValue("@TelefCli", reg.telCli);
                int c = cmd.ExecuteNonQuery();
                mensaje = $"Se ha actualizado {c} cliente(s) : {reg.nomCli}";
            }
            catch (SqlException ex) { mensaje = ex.Message; }
            finally { cn.getcn.Close(); }
            return mensaje;
        }

        //buscar
        public Cliente Buscar(string codCli)
        {
            return listado().FirstOrDefault(x => x.codCli == codCli);
        }

        //eliminar
        public string Eliminar(string codCli) 
        {
            string mensaje = "";
            try
            {
                cn.getcn.Open();
                SqlCommand cmd = new SqlCommand("usp_EliminarCliente", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCli", codCli);
                int c = cmd.ExecuteNonQuery();
                mensaje = $"Se ha eliminado {c} cliente(s) : "+codCli;
            }
            catch (SqlException ex) { mensaje = ex.Message; }
            finally { cn.getcn.Close(); }
            return mensaje;
        }
    }
}
