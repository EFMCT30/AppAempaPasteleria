using AppAempaPasteleria.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppAempaPasteleria.Modulos
{
    public class productosDAO
    {
        private conexionDAO cn = new conexionDAO();

        //listar
        public IEnumerable<Producto> listado()
        {
            List<Producto> temporal = new List<Producto>();
            try
            {
                cn.getcn.Open();
                SqlCommand cmd = new SqlCommand("usp_listarProductos", cn.getcn);
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    temporal.Add(new Producto()
                    {
                        idProd = rd.GetString(0),
                        nomProd = rd.GetString(1),
                        fotoProd= rd.GetString(2),
                        idIn = rd.GetString(3),
                        idCate = rd.GetString(4),
                        idCateD = rd.GetString(5),
                        descProd = rd.GetString(6),
                        preProd = rd.GetDecimal(7),
                        stockProd = rd.GetInt16(8),
                    });
                }
                rd.Close();
            }
            catch (SqlException ex) { temporal = new List<Producto>(); }
            finally { cn.getcn.Close(); }
            return temporal;
        }

        //insertar
        public string Agregar(Producto reg)
        {
            string mensaje = "";
            try
            {
                cn.getcn.Open();
                SqlCommand cmd = new SqlCommand("usp_AgregarProductos", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nomProd", reg.nomProd);
                cmd.Parameters.AddWithValue("@fotoProd", reg.fotoProd);
                cmd.Parameters.AddWithValue("@idIn", reg.idIn);
                cmd.Parameters.AddWithValue("@idCate", reg.idCate);
                cmd.Parameters.AddWithValue("@descProd", reg.descProd);
                cmd.Parameters.AddWithValue("@preProd", reg.preProd);
                cmd.Parameters.AddWithValue("@stockProd", reg.stockProd);
                int c = cmd.ExecuteNonQuery();
                mensaje = $"Se ha agregado {c} producto(s) : {reg.nomProd}";
            }
            catch (SqlException ex) { mensaje = ex.Message; }
            finally { cn.getcn.Close(); }
            return mensaje;
        }

        //actualizar
        public string Actualizar(Producto reg)
        {
            string mensaje = "";
            try
            {
                cn.getcn.Open();
                SqlCommand cmd = new SqlCommand("usp_EditarProductos", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProd", reg.idProd);
                cmd.Parameters.AddWithValue("@nomProd", reg.nomProd);
                cmd.Parameters.AddWithValue("@fotoProd", reg.fotoProd);
                cmd.Parameters.AddWithValue("@idIn", reg.idIn);
                cmd.Parameters.AddWithValue("@idCate", reg.idCate);
                cmd.Parameters.AddWithValue("@descProd", reg.descProd);
                cmd.Parameters.AddWithValue("@preProd", reg.preProd);
                cmd.Parameters.AddWithValue("@stockProd", reg.stockProd);
                int c = cmd.ExecuteNonQuery();
                mensaje = $"Se ha actualizo {c} producto(s) : {reg.nomProd}";
            }
            catch (SqlException ex) { mensaje = ex.Message; }
            finally { cn.getcn.Close(); }
            return mensaje;
        }

        //buscar
        public Producto Buscar(string idProd)
        {
            return listado().FirstOrDefault(x => x.idProd == idProd);
        }

        //eliminar
        public string Eliminar(string idProd)
        {
            string mensaje = "";
            try
            {
                cn.getcn.Open();
                SqlCommand cmd = new SqlCommand("usp_EliminarProductos", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProd", idProd);

                int c = cmd.ExecuteNonQuery();
                mensaje = $"Se ha eliminado {c} Producto(s) : " + idProd;
            }
            catch (SqlException ex) { mensaje = ex.Message; }
            finally { cn.getcn.Close(); }
            return mensaje;
        }
    }
}
