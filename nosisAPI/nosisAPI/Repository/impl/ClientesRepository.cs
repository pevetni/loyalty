using System;
using nosisAPI.Config;
using nosisAPI.Models;

namespace nosisAPI.Repository.impl
{
    public class ClientesRepository : RepositoryBase<Clientes>, IClientesRepository
    {
        public ClientesRepository(RepositorySQLContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
