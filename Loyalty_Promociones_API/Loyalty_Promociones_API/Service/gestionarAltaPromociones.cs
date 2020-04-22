using Loyalty_Promociones_API.Models;
using Loyalty_Promociones_API.postPromociones;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Loyalty_Promociones_API.Service
{
    public class gestionarAltaPromociones
    {
        public Response altaPromociones (Root json, Token token)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            postCardAssignCatalog postCardAssign;
            Response responseContent;
            postCardAssign = new postCardAssignCatalog();
            string AltaPromociones = string.Empty;

            AltaPromociones = configuration["AltaPromociones"];

            try 
            {
                var jsonString = JsonConvert.SerializeObject(json);
                var data = new StringContent(jsonString, Encoding.UTF8, "application/json");

                responseContent = SyncHelper.RunSync<Response>(() => postCardAssign.postCardAssingCatalogAsync(data, AltaPromociones, token));
                
                return responseContent;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
    }
}
