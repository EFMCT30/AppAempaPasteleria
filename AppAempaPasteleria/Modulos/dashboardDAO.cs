using AppAempaPasteleria.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AppAempaPasteleria.Modulos
{
    public class dashboardDAO
    {
        conexionDAO cn= new conexionDAO();
        public IEnumerable<Dashboard> ObtenerResumenVenta()
        {
            List<Dashboard> resumenVentas = new List<Dashboard>();
            try
            {
                cn.getcn.Open();
                SqlCommand cmd = new SqlCommand("usp_ResumenVenta", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    resumenVentas.Add(new Dashboard()
                    {
                        Fecha = rd.GetDateTime(0),
                        TotalPedidos = rd.GetInt32(1)
                    });
                }
                rd.Close();
            }
            catch (SqlException ex)
            {
                resumenVentas = new List<Dashboard>();
            }
            finally
            {
                cn.getcn.Close();
            }
            return resumenVentas;
        }

        public IEnumerable<Producto> ObtenerResumenProductos()
        {
            List<Producto> resumenProductos = new List<Producto>();

            using (cn.getcn)
            {
                cn.getcn.Open();

                SqlCommand command = new SqlCommand("usp_ResumenProducto", cn.getcn);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Producto producto = new Producto()
                    {
                        nomProd = reader.GetString(0),
                        stockProd = reader.GetInt32(1)
                    };

                    resumenProductos.Add(producto);
                }

                reader.Close();
            }

            return resumenProductos;
        }

        public IEnumerable<Cliente> ObtenerResumenVentaxCli()
        {
            List<Cliente> resumenVentas = new List<Cliente>();

            using (cn.getcn)
            {
                cn.getcn.Open();

                SqlCommand command = new SqlCommand("usp_ResumenVentaxCli", cn.getcn);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Cliente venta = new Cliente()
                    {
                        nomCli = reader.GetString(0),
                        totalPedidos = reader.GetInt32(1)
                    };

                    resumenVentas.Add(venta);
                }

                reader.Close();
            }

            return resumenVentas;
        }

    }
}
