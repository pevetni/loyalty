using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using nosisAPI.Config;
using nosisAPI.Models;
using nosisAPI.Repository;
using nosisAPI.Services;

namespace nosisAPI.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class NosisAPIController : ControllerBase
    {

        private readonly ILogger<NosisAPIController> _logger;
        private readonly ILogger<ClienteNosis> _loggerCliente;
        private readonly ClienteNosis _clienteNosis;
        private readonly IMongoResponseNosisRepository _mongoResponseNosisRepository;
        private IRepositoryWrapper _repoWrapper;

        public NosisAPIController(ILogger<NosisAPIController> logger, ILogger<ClienteNosis> loggerCliente, IMongoResponseNosisRepository mongoResponseNosisRepository, IRepositoryWrapper repoWrapper)
        {
            _logger = logger;
            _mongoResponseNosisRepository = mongoResponseNosisRepository;
            _clienteNosis = new ClienteNosis(loggerCliente, _mongoResponseNosisRepository);
            _repoWrapper = repoWrapper;
        }

        /// <summary>
        /// Obtiene los valores retornados por NOSIS en formato json.
        /// </summary>
        /// <param name="dni"></param>
        [HttpGet("/validar/{dni}", Name = "validar")]
        public IActionResult GetNosisResponse(string dni)
        {
            _logger.LogInformation("Solicitando datos de nosis");
            ResponseNosis responseNosis = _clienteNosis.callNosis(dni);
            return StatusCode(Int16.Parse(responseNosis.Contenido.Resultado.Estado), responseNosis); 
        }

        [HttpGet("/sincronizar", Name = "sincronizar")]
        public IActionResult sincronizar()
        {
            _logger.LogInformation("Iniciado el proceso de sincronización de registros de la BD contra Nosis");
            _clienteNosis.sincronizacion(_repoWrapper);
            return StatusCode(200, "Proceso de sincronización de registros culminado!!");
        }

        [HttpGet("/nosis", Name = "status")]
        public IActionResult GetStatus()
        {
            _logger.LogInformation("Servicio levantado con exito!");
            return StatusCode(200, "Servicio levantado");
        }
    }
}
