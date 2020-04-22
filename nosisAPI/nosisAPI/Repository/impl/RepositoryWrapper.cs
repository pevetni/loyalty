using System;
using nosisAPI.Config;

namespace nosisAPI.Repository.impl
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositorySQLContext _repoContext;
        private IClientesRepository _clientes;

        public IClientesRepository Clientes
        {
            get
            {
                if (_clientes == null)
                {
                    _clientes = new ClientesRepository(_repoContext);
                }

                return _clientes;
            }
        }

        public RepositoryWrapper(RepositorySQLContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
