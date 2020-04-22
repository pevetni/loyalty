using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Loyalty_Promociones_API.Models
{
    public class Database
    {
        public string CadenaConexion()
        {
            string server = string.Empty;
            string database = string.Empty;
            string user = string.Empty;
            string password = string.Empty;

            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            server = configuration["serverDB"];
            database = configuration["dataBase"];
            user = configuration["userDB"];
            password = configuration["passDB"];

            try
            {
                string connStr = "Server=" + server +
                                    "; Database=" + database +
                                    "; User id=" + user +
                                    "; Password=" + password + ";";
                return connStr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

