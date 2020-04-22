using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Loyalty_Promociones_API.Service
{
    public class gestionarToken
    {
        public Token obtenerToken ()
        {
            string UserName = string.Empty;
            string Password = string.Empty;
            string Scope = string.Empty;
            string Authorization = string.Empty;
            string Grant_Type = string.Empty;
            string TokenPromociones = string.Empty;            

            getToken token;
            Token responseToken;
            Credenciales credencial;            

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            try 
            {
                UserName = configuration["username"];
                Password = configuration["password"];
                Scope = configuration["scope"];
                Authorization = configuration["authorization"];
                Grant_Type = configuration["grant_type"];
                TokenPromociones = configuration["TokenPromociones"];

                credencial = new Credenciales();
                credencial.scope = Scope;
                credencial.username = UserName;
                credencial.grant_type = Grant_Type;
                credencial.password = Password;
                credencial.authorization = Authorization;
                credencial.uriToken = TokenPromociones;

                responseToken = new Token();
                token = new getToken();

                responseToken = SyncHelper.RunSync<Token>(() => token.getTokenAsync(credencial));

                return responseToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
