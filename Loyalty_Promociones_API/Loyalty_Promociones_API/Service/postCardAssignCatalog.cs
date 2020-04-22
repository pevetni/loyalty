using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Loyalty_Promociones_API.postPromociones;
using Loyalty_Promociones_API.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Data.SqlClient;
using System.Data;

namespace Loyalty_Promociones_API.Service
{
    public class postCardAssignCatalog
    {
        public async Task<Response> postCardAssingCatalogAsync(HttpContent data, string url, Token token)
        {
            string result;
            Response responseContent;            

            try
            {        
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

                    var response = await httpClient.PostAsync(url, data);
                    
                    result = response.Content.ReadAsStringAsync().Result;

                    responseContent = JsonConvert.DeserializeObject<Response>(result);
                    
                    return responseContent;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public Int32 grabarAltaPendiete (Root cliente, string json)
        {
            Database database = new Database();
            SqlConnection conn = new SqlConnection(database.CadenaConexion());
            conn.Open();
            int output;

            try
            {
                SqlCommand cmd = new SqlCommand("LOYALTY.sp_Promociones_Pendiente_Insert", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@tipoDocumentoCliente", cliente.items[0].identifier);
                cmd.Parameters.AddWithValue("@documentoCliete", cliente.items[0].idType);
                cmd.Parameters.AddWithValue("@request", json);
                cmd.Parameters.Add("@output", SqlDbType.Int);
                cmd.Parameters["@Output"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                output = Convert.ToInt32(cmd.Parameters["@output"].Value);

                return output;
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


