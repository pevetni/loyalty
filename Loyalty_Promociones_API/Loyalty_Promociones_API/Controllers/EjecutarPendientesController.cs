using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Loyalty_Promociones_API.Models;
using Loyalty_Promociones_API.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Loyalty_Promociones_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EjecutarPendientesController : ControllerBase
    {
        [HttpPost()]
        public IActionResult EjecutarAltaPendientes()
        {
            List<Pendientes> listPendientes = new List<Pendientes>();
            ejecutarAltasPendientes pendientes = new ejecutarAltasPendientes();

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            string AltaPromociones = string.Empty;            
            postCardAssignCatalog postCardAssign;
            Response responseContent;
            Token responseToken;
            int pendientesEjecutadas = 0;

            try
            {
                AltaPromociones = configuration["AltaPromociones"];
                postCardAssign = new postCardAssignCatalog();                
                gestionarToken token = new gestionarToken();
                
                listPendientes = pendientes.ObtenerAltasPendientes();
                responseToken = token.obtenerToken();

                if (responseToken.AccessToken != null)
                { 
                    foreach (Pendientes pend in listPendientes)
                    {
                        var data = new StringContent(pend.json, Encoding.UTF8, "application/json");

                        Logger.LoggerMessage("Promociones API (pendientes): " + pend.json);
                        responseContent = SyncHelper.RunSync<Response>(() => postCardAssign.postCardAssingCatalogAsync(data, AltaPromociones, responseToken));

                        if (responseContent != null && responseContent.StatusCode == 200)
                        {                            
                            pendientes.ActualizarEjecucionPendiente(pend);
                            pendientesEjecutadas += 1;
                        }
                    }

                    if (listPendientes.Count == 0)
                    {
                        Logger.LoggerMessage("Promociones API (pendientes): 200 - No se encontraron altas pendientes de ejecutar");
                        return StatusCode(200, "No se encontraron altas pendientes de ejecutar");
                    }
                    else if (pendientesEjecutadas == listPendientes.Count)
                    {
                        Logger.LoggerMessage("Promociones API (pendientes): 200 - Se ejecutaron " + pendientesEjecutadas + " altas pendientes de un total de " + listPendientes.Count);
                        return StatusCode(200, "200 - Se ejecutaron " + pendientesEjecutadas + " altas pendientes de un total de " + listPendientes.Count);
                    }
                    else
                    {
                        Logger.LoggerMessage("Promociones API (pendientes): 202 - Se ejecutaron " + pendientesEjecutadas + " altas pendientes de un total de " + listPendientes.Count);
                        return StatusCode(202, "202 - Se ejecutaron " + pendientesEjecutadas + " altas pendientes de un total de " + listPendientes.Count);
                    }                    
                }
                else
                {
                    Logger.LoggerMessage("Promociones API (pendientes): 401 - Error al obtener Token.");
                    return StatusCode(401, "Error al obtener Token.");
                }
            }            
            catch (Exception ex)
            {
                Logger.LoggerMessage("Promociones API (pendientes): 480 - Excepcion no controlada: " + ex.Message.ToString() + " - " + ex.InnerException.ToString());
                return StatusCode(480, "Excepcion no controlada: " + ex.Message.ToString() + " - " + ex.InnerException.ToString());
            }
        }
    }
}