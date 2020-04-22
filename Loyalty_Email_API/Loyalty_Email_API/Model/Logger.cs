using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Loyalty_Email_API
{
    public static class Logger
    {
        public static void LoggerMessage(string message)
        {
            Database database = new Database();
            SqlConnection conn = new SqlConnection(database.CadenaConexion());
            conn.Open();

            try
            {
                SqlCommand cmd = new SqlCommand("LOYALTY.sp_Logs_Insert", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mensaje", message);
                cmd.ExecuteNonQuery();
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
    }
}
