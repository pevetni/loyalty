using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Loyalty_Promociones_API.Service;
using Loyalty_Promociones_API.postPromociones;
using Loyalty_Promociones_API.Models;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Linq;

namespace Loyalty_Promociones_API.Controllers
{
    [ApiController]
    [Route("api/PromocionesController")]
    public class PromocionesController : ControllerBase
    {
        [HttpPost()]
        public IActionResult AltaPromociones(Cliente cliente)
        {           
            Token responseToken;
            Response response = new Response();
            gestionarToken token = new gestionarToken();
            gestionarAltaPromociones alta = new gestionarAltaPromociones();
            postCardAssignCatalog postCardAssign = new postCardAssignCatalog();
            string prueba = string.Empty;
            int altaPendiente = 0;
            string ReturnCodePendientes = string.Empty;

            try
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                var configuration = builder.Build();

                ReturnCodePendientes = configuration["ReturnCodePendientes"];
                string[] returnCodesPendientes = ReturnCodePendientes.Split(',');

                prueba = configuration["prueba"];

                if (prueba == "S")
                {
                    return StatusCode(200, "Alta exitosa");
                }                

                Root ro = new Root();
                ro.catalog = cliente.Catalog;
                ro.companyId = cliente.CompanyId;
                List<Item> item = new List<Item>()
                    {
                        new Item {
                            operation = "I",
                            id = cliente.CardId,
                            customer = cliente.CustomerId,
                            amount = cliente.Amount,
                            name = cliente.NombreCliente,
                            surname = cliente.ApellidoCliente,
                            idType = cliente.TipoIdentificacion,
                            identifier = cliente.Identificacion
                        }
                    };
                ro.items = item;
                List<Params> parameters = new List<Params>()
                    {
                        new Params {
                            cardType = cliente.CardTye,
                            contract = cliente.Contract
                        }
                    };
                ro.@params = parameters;

                var jsonString = JsonConvert.SerializeObject(ro);
                
                Logger.LoggerMessage("Promociones API: " + jsonString);
                responseToken = token.obtenerToken();

                if (responseToken.AccessToken != null)
                {
                    response = alta.altaPromociones(ro, responseToken);
                    if (response != null && (returnCodesPendientes.Contains(response.StatusCode.ToString())))
                    {
                        altaPendiente = postCardAssign.grabarAltaPendiete(ro, jsonString);
                        switch (altaPendiente)
                        {
                            case 1:
                                response.Detail.Detail = response.Detail.Detail + " - El alta ha quedado en pendiente en Promociones";
                                break;
                            case 2:
                                response.Detail.Detail = response.Detail.Detail + " - Existe un alta pendiente para ejecutar en Promociones para el documento " + ro.items[0].identifier;
                                break;
                            default:
                                break;
                        }
                        Logger.LoggerMessage("Promociones API: " + response.StatusCode + " - " + response.Detail.Detail);
                        return StatusCode(202, response.StatusCode + " - " + response.Detail.Detail);
                    }
                    else
                    {
                        Logger.LoggerMessage("Promociones API: " + response.StatusCode + " - " + response.Detail.Detail);
                        return StatusCode(response.StatusCode, response.Detail.Detail);
                    }                    
                }
                else
                {
                    altaPendiente = postCardAssign.grabarAltaPendiete(ro, jsonString);
                    
                    switch (altaPendiente)
                    {
                        case 1:
                            response.Detail.Detail = response.Detail.Detail + " - El alta ha quedado en pendiente en Promociones";
                            break;
                        case 2:
                            response.Detail.Detail = response.Detail.Detail + " - Existe un alta pendiente para ejecutar en Promociones para el documento " + ro.items[0].identifier;
                            break;
                        default:
                            break;
                    }
                    Logger.LoggerMessage("Promociones API: " + response.StatusCode + " - " + response.Detail.Detail);
                    return StatusCode(202, "Error al obtener Token - " + response.Detail.Detail);
                }
            }
            catch (Exception ex)
            {
                Logger.LoggerMessage("Promociones API: 480 - Excepcion no controlada: " + ex.Message.ToString() + " - " + ex.InnerException.ToString());
                return StatusCode(480, "Promociones API: Excepcion no controlada: " + ex.Message.ToString() + " - " + ex.InnerException.ToString());
            }
        }
    }
}   



