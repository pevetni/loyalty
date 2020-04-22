using Loyalty_Promociones_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Loyalty_Promociones_API.Service
{
    public class ejecutarAltasPendientes
    {
        internal List<Pendientes> ObtenerAltasPendientes ()
        { 
        List<Pendientes> AltasPendientes = new List<Pendientes>();
        Database database = new Database();
        SqlConnection conn = new SqlConnection(database.CadenaConexion());

        try
        {
            conn.Open();

            SqlCommand cmd = new SqlCommand("LOYALTY.sp_Promociones_Pendientes_Select", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                AltasPendientes.Add(new Pendientes(int.Parse(reader[0].ToString()), reader[1].ToString(), reader[2].ToString(), reader[3].ToString()));
            }
                return AltasPendientes;
            }
            catch (SqlException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        internal void ActualizarEjecucionPendiente (Pendientes pendiente)
        {
            Database database = new Database();
            SqlConnection conn = new SqlConnection(database.CadenaConexion());
            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("LOYALTY.sp_Promociones_Pendiente_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@documentoCliete", pendiente.documentoCliente);
                cmd.Parameters.AddWithValue("@tipoDocumentoCliente", pendiente.tipoDocumentoCliente);

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
