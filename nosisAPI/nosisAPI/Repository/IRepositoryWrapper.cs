using System;
using nosisAPI.Repository;

namespace nosisAPI.Config
{
    public interface IRepositoryWrapper
    {
        IClientesRepository Clientes { get; }
        void Save();
    }
}
