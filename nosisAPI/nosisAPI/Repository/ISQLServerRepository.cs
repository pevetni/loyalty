using System;
using nosisAPI.Models;

namespace nosisAPI.Repository
{
    public interface ISQLServerRepository
    {
        public void getAllNotSynchronizedClients();

        public bool sinchronizeClientWithNosis(Clientes cliente);
    }
}
