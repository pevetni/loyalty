using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using nosisAPI.Exceptions;
using nosisAPI.Models;
using System.Configuration;
using Microsoft.Extensions.Logging;
using nosisAPI.Repository;
using System.Collections.Generic;
using nosisAPI.Config;

namespace nosisAPI.Services
{
    public class ClienteNosis
    {
        private readonly ILogger<ClienteNosis> _logger;
        private readonly IMongoResponseNosisRepository _mongoResponseNosisRepository;
        private readonly ISQLServerRepository _sqlServerRepository;

        public ClienteNosis(ILogger<ClienteNosis> logger, IMongoResponseNosisRepository mongoResponseNosisRepository)
        {
            _logger = logger;
            _mongoResponseNosisRepository = mongoResponseNosisRepository;
        }

        public ClienteNosis(ILogger<ClienteNosis> logger, ISQLServerRepository sqlServerRepository)
        {
            _logger = logger;
            _sqlServerRepository = sqlServerRepository;
        }

        public ResponseNosis callNosis(string dni, bool persist = true)
        {
            ResponseNosis responseNosis = null;
            try { 
                string URL = ConfigurationManager.AppSettings["URL"];
                string urlParameters = "?usuario=" + ConfigurationManager.AppSettings["usuario"] + "&token=" + ConfigurationManager.AppSettings["token"] + "&documento=" + dni + "&VR=" + ConfigurationManager.AppSettings["VR"];

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);

                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
                _logger.LogInformation("Request to: {0}{1}",URL , urlParameters);
                HttpResponseMessage response = client.GetAsync(urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {
                    string xml = response.Content.ReadAsStringAsync().Result;
                    responseNosis = new ResponseNosis();
                    responseNosis = JsonConvert.DeserializeObject<ResponseNosis>(xml);
                    _logger.LogInformation("Response Success: {0}", responseNosis);
                    //Add Json to DB
                    if (persist)
                    {
                        try
                        {
                            MongoResponseNosis mongoResponseNosis = new MongoResponseNosis();
                            mongoResponseNosis.Contenido = responseNosis.Contenido;
                            _mongoResponseNosisRepository.CreateClienteNosisAsync(mongoResponseNosis);
                        }
                        catch (Exception e)
                        {
                            _logger.LogError("Error al tratar de guardar el registro en la BD mongo: {0}", e.Message);
                        }
                    }
                }
                else {
                    responseNosis = new ResponseNosis();
                    responseNosis.Contenido.Resultado.Estado = response.StatusCode.ToString();
                    responseNosis.Contenido.Resultado.Novedad = "Existe un problema de comunicacion con Nosis, por favor descarte problemas de red o contacte con el proveedor";
                }
            }
            catch (UserDidntExistException e) {
                //Marcar BD como que no existe el usuario
                _logger.LogError("Error: {0}", e);
            }

            return responseNosis;
        }

        public void sincronizacion(IRepositoryWrapper _repoWrapper) {
            using (IEnumerator<Clientes> empEnumerator = _repoWrapper.Clientes.FindByCondition(x => !x.ValidadoNosis).GetEnumerator())
            {
                while (empEnumerator.MoveNext())
                {
                    // now empEnumerator.Current is the Employee instance without casting
                    Clientes cli = empEnumerator.Current;
                    ResponseNosis resp = callNosis(cli.DNI.ToString(), false);
                    if (resp != null && resp.Contenido!= null && resp.Contenido.Datos != null ) { 
                        foreach (Variables variable in resp.Contenido.Datos.Variables)
                        {
                            if (variable.Nombre.Contains("Nombre"))
                            {
                                cli.Nombre = variable.Valor;
                            }
                            if (variable.Nombre.Contains("Apellido"))
                            {
                                cli.Apellido = variable.Valor;
                                cli.ValidadoNosis = true;
                                break;
                            }
                        }
                        _repoWrapper.Clientes.Update(cli);                        
                    }
                    else
                    {
                        _logger.LogInformation("Cliente no sincronizado, falló la comunicación con NOSIS: {0} ", cli);
                    }
                    
                }
                _repoWrapper.Save();
                _logger.LogInformation("Cliente Sincronizado con éxito!");
            }
        }
    }
}
