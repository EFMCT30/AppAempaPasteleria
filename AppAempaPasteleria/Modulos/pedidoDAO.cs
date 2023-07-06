using AppAempaPasteleria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AppAempaPasteleria.Modulos
{
    public class pedidoDAO
    {
        private conexionDAO cn = new conexionDAO();

        //listar
        public IEnumerable<Pedido> listado()
        {
            List<Pedido> temporal = new List<Pedido>();
            try
            {
                cn.getcn.Open(); ;
                SqlCommand cmd = new SqlCommand("usp_ListarPedido", cn.getcn);
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    temporal.Add(new Pedido()
                    {
                        idPed = rd.GetInt32(0),
                        fechaPed = rd.GetDateTime(1),
                        IdPro = rd.GetString(2),
                        NompPro = rd.GetString(3),
                        idCli = rd.GetString(4),
                        NomCli = rd.GetString(5),
                        metodoPed = rd.GetString(6),
                        EntregaPed = rd.GetString(7),
                        EstadoPed = rd.GetString(8),
                        precioU = rd.GetDecimal(9),
                        Cantidad = rd.GetInt32(10),
                        monto = rd.GetDecimal(11),
                    });
                }
                rd.Close();
            }
            catch (SqlException ex) { temporal = new List<Pedido>(); }
            finally { cn.getcn.Close(); }
            return temporal;
        }

        public Pedido Buscar(int idPed)
        {
            return listado().FirstOrDefault(x => x.idPed == idPed);
        }

        public string Actualizar(Pedido reg)
        {
            string mensaje = "";
            try
            {
                cn.getcn.Open();
                SqlCommand cmd = new SqlCommand("usp_EditarPedidoEsta", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idPed", reg.idPed);
                cmd.Parameters.AddWithValue("@idCli", reg.idCli);
                cmd.Parameters.AddWithValue("@metodoPed", reg.metodoPed);
                cmd.Parameters.AddWithValue("@EntregaPed", reg.EntregaPed);
                cmd.Parameters.AddWithValue("@EstadoPed", reg.EstadoPed);
                int c = cmd.ExecuteNonQuery();
                mensaje = $"Se ha actualizo el estado del pedido : {reg.idPed}";
            }
            catch (SqlException ex) { mensaje = ex.Message; }
            finally { cn.getcn.Close(); }
            return mensaje;
        }

        //eliminar
        public string Eliminar(int idPed)
        {
            string mensaje = "";
            try
            {
                cn.getcn.Open();
                SqlCommand cmd = new SqlCommand("usp_EliminarPedido", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idPed", idPed);

                int c = cmd.ExecuteNonQuery();
                mensaje = $"Se ha eliminado {c} Pedido(s) : " + idPed;
            }
            catch (SqlException ex) { mensaje = ex.Message; }
            finally { cn.getcn.Close(); }
            return mensaje;
        }
    }
}
