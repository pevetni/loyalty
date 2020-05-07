using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ObjetosNegocio.Datos;
using System.Data.SqlClient;
using System.Globalization;
using System.Security.Cryptography;
using SGM_LOYALTY.Utilities;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace SGM_LOYALTY
{
    public partial class ACTIVATE : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                using (IBaseDatos baseDatos = BaseDatos.Construir(new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["STR-CNN-LOYALTY"])))
                {
                    string[] param = new string[] { "Mensaje" };
                    object[] value = new object[1];
                    value[0] = "ACCESO: ingreso a la página";

                    baseDatos.EjecutarSP("[LOYALTY].[sp_Logs_Insert]", param, value);
                };

                hfOperacion.Value = "nuevo";
            }
            else
            {
                //Esto es por si el usuario hace F5 y la página no vuelva a enviar los datos nuevamente (re summit), se evita mostrando la página en sus estado inicial.
                bool valor;

                if (Session["InformacionRegistrada"]!= null && bool.TryParse(Session["InformacionRegistrada"].ToString(), out valor))
                {
                    if (valor)
                    {
                        Response.Redirect(Request.Path);
                    }
                }                
            }
        }

        [WebMethod]
        public static List<Ciudades> GetProvincias()
        {
            List<Ciudades> lst = new List<Ciudades>();
            DataTable dt = Provincias();

            foreach (DataRow item in dt.Rows)
            {
                Ciudades myc = new Ciudades();
                myc.Id = Convert.ToInt32(item[0].ToString());
                myc.Nombre = item[1].ToString();
                lst.Add(myc);
            }

            return lst;
        }

        [WebMethod]
        public static List<Ciudades> GetCiudades(string Provincia)
        {
            List<Ciudades> lst = new List<Ciudades>();
            DataTable dt = Ciudades(Provincia);

            foreach (DataRow item in dt.Rows)
            {
                Ciudades myc = new Ciudades();
                myc.Id = Convert.ToInt32(item[0].ToString());
                myc.Nombre = item[1].ToString();
                lst.Add(myc);
            }

            return lst;
        }

        [WebMethod]
        public static List<Codigos> GetGrupos(string Codigo)
        {
            List<Codigos> lst = new List<Codigos>();
            DataTable dt = Grupos(Codigo);

            foreach (DataRow item in dt.Rows)
            {
                Codigos myc = new Codigos();
                myc.Codigo = item[0].ToString();
                lst.Add(myc);
            }

            return lst;
        }

        [WebMethod]
        public static List<Codigos> GetTarjetas(string Codigo)
        {
            List<Codigos> lst = new List<Codigos>();
            DataTable dt = Tarjetas(Codigo);

            foreach (DataRow item in dt.Rows)
            {
                Codigos myc = new Codigos();
                myc.Codigo = item[0].ToString();
                lst.Add(myc);
            }

            return lst;
        }


        private static DataTable Provincias()
        {
            DataTable resultado = new DataTable();
            string[] param = new string[0];
            object[] value = new object[0];
            try
            {
                using (IBaseDatos baseDatos = BaseDatos.Construir(new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["STR-CNN-LOYALTY"])))
                {
                    baseDatos.EjecutarSP(resultado, "[LOYALTY].[sp_Provincias_Select]", param, value);
                }
                return resultado;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static DataTable Ciudades(string provincia)
        {
            DataTable resultado = new DataTable();

            string[] param = new string[] { "PROVINCIA" };
            object[] value = new object[1];
            value[0] = provincia;
            try
            {
                using (IBaseDatos baseDatos = BaseDatos.Construir(new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["STR-CNN-LOYALTY"])))
                {
                    baseDatos.EjecutarSP(resultado, "[LOYALTY].[sp_Ciudades_Select]", param, value);
                }

                return resultado;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static DataTable Grupos(string Codigo)
        {
            DataTable resultado = new DataTable();
            string[] param = new string[] { "Codigo" };
            object[] value = new object[1];
            value[0] = Codigo;
            try
            {
                using (IBaseDatos baseDatos = BaseDatos.Construir(new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["STR-CNN-LOYALTY"])))
                {
                    baseDatos.EjecutarSP(resultado, "[LOYALTY].[sp_Grupos_Existe]", param, value);
                }
                return resultado;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static DataTable Tarjetas(string Codigo)
        {
            DataTable resultado = new DataTable();
            string[] param = new string[] { "Codigo" };
            object[] value = new object[1];
            value[0] = Codigo;
            try
            {
                using (IBaseDatos baseDatos = BaseDatos.Construir(new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["STR-CNN-LOYALTY"])))
                {
                    baseDatos.EjecutarSP(resultado, "[LOYALTY].[sp_Tarjetas_Existe]", param, value);
                }
                return resultado;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        protected void btnSend_Click(object sender, EventArgs e)
        {
            string[] param = new string[] { "DNI", "Apellido", "Nombre", "Genero", "FechaNacimiento", "Telefono", "Celular", "Direccion", "Postal", "Provincia", "ProvinciaNombre", "Ciudad", "CiudadNombre", "Email", "Grupo", "Tarjeta", "HashValidacion", "Enviado","Validado" };
            object[] value = new object[19];
            value[0] = this.hfDNI.Value.ToString();
            value[1] = this.hfApellido.Value.ToString();
            value[2] = this.hfNombre.Value.ToString();
            value[3] = this.hfGenero.Value.ToString();

            DateTime dtFecha = DateTime.ParseExact(this.hfFechaNacimiento.Value.ToString(), "yyyy-MM-d", CultureInfo.InvariantCulture);
            string fecha = dtFecha.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            value[4] = dtFecha;

            value[5] = this.hfTelefono.Value.ToString();
            value[6] = this.hfCelular.Value.ToString();
            value[7] = this.hfDireccion.Value.ToString();
            value[8] = this.hfPostal.Value.ToString();
            if (this.hfProvincia.Value.ToString().ToUpper() == ("Seleccione una provincia").ToUpper())
            {
                value[9] = "0";
                value[10] = "";
            }
            else
            {
                value[9] = this.hfProvincia.Value.ToString();
                value[10] = this.hfProvinciaNombre.Value.ToString();
            }
            if (this.hfCiudad.Value.ToString().ToUpper() == ("Seleccione una ciudad").ToUpper())
            {
                value[11] = "0";
                value[12] = "";
            }
            else
            {
                value[11] = this.hfCiudad.Value.ToString();
                value[12] = this.hfCiudadNombre.Value.ToString();
            }

            value[13] = this.hfEmail.Value.ToString();
            value[14] = this.hfGrupo.Value.ToString();
            value[15] = this.hfTarjeta.Value.ToString();
            value[16] = Helper.EncodeString(this.hfDNI.Value.ToString());
            value[17] = 0; 
            value[18] = this.hfValidado.Value.ToString();

            /*****************
             * Envio de mail *
             * ***************/

            Email email = new Email();
            email.NombreRemitente = value[1].ToString() + " " + value[2].ToString();
            email.AsuntoMail = ConfigurationManager.AppSettings["AsuntoEmail"];
            string cuerpoMail = Mail.CrearCuerpoMailValidacion(Server.UrlEncode(Helper.EncodeString(hfDNI.Value.ToString())),
                                                              value[2] + " " + value[1],
                                                              urlValidacion: ConfigurationManager.AppSettings["UrlValidacionMail"]);
            email.CuerpoMail = cuerpoMail;
            email.Destinatario = value[13].ToString();
            email.IsBodyHtml = ConfigurationManager.AppSettings["isBodyHtmlEmail"].Equals("True");

            /************************
             * Enviar datos a PROMO *
             * **********************/

            Promociones promo = new Promociones();
            promo.Catalog = "CatalogCardAssign";
            promo.CompanyId = "diarco";
            promo.CardId = value[15].ToString();
            promo.CardTye = value[15].ToString().Substring(value[15].ToString().Length - 4);
            promo.Contract = value[14].ToString();
            promo.CustomerId = value[0].ToString();
            promo.NombreCliente = value[2].ToString();
            promo.ApellidoCliente = value[1].ToString();
            promo.TipoIdentificacion = "DNI";
            promo.Identificacion = value[0].ToString();
            promo.Amount = 0.0D;


            if (ConfigurationManager.AppSettings["EmailEnable"].Equals("True") && EnviarMailActivacion(email))
            {
                value[17] = 1;
            }

            DataTable resultado = new DataTable();
            try
            {
                using (IBaseDatos baseDatos = BaseDatos.Construir(new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["STR-CNN-LOYALTY"])))
                {
                    baseDatos.EjecutarSP(resultado, "[LOYALTY].[sp_Clientes_Insert]", param, value);

                    foreach (DataRow item in resultado.Rows)
                    {
                        if (item[0].ToString() == "SI")
                        {
                            hfOperacion.Value = "ok";
                            if (ConfigurationManager.AppSettings["PromoEnable"].Equals("True"))
                                EnviarAPromociones(promo);
                        }
                        else
                        {
                            this.lblError.Text = item[1].ToString();
                            hfOperacion.Value = "error";
                        }
                    }
                }

               

                lblMensajeFinRegistracion.Text = "Muchas Gracias por Registrarte, recibirás un mensaje en " + value[13].ToString() + " con las instrucciones para activar tu tarjeta.";

                Session["InformacionRegistrada"] = true;

                return;
            }
            catch (Exception ex)
            {
                hfOperacion.Value = ex.Message;
            }
        }

        public bool EnviarMailActivacion(Email email)
        {
            try
            {
                string URL = ConfigurationManager.AppSettings["URLEmail"];

                HttpClient client = new HttpClient();
                var data = new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json");

                // List data response.
                HttpResponseMessage response = client.PostAsync(URL, data).Result;

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    using (IBaseDatos baseDatos = BaseDatos.Construir(new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["STR-CNN-LOYALTY"])))
                    {
                        string[] param = new string[] { "Mensaje" };
                        object[] value = new object[1];
                        value[0] = "RETRY: servicio de Mail envio Status code = " + response.StatusCode + " con el siguiente mensaje = "+ response.RequestMessage.Content;

                        baseDatos.EjecutarSP("[LOYALTY].[sp_Logs_Insert]", param, value);
                    };
                    return false;
                }
            }
            catch (Exception e) {
                using (IBaseDatos baseDatos = BaseDatos.Construir(new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["STR-CNN-LOYALTY"])))
                {
                    string[] param = new string[] { "Mensaje" };
                    object[] value = new object[1];
                    value[0] = "ERROR: servicio de Envio de MAIL no accesible, = " + e.Message;

                    baseDatos.EjecutarSP("[LOYALTY].[sp_Logs_Insert]", param, value);
                };
                return false;
            }
        }

        public bool EnviarAPromociones(Promociones promo)
        {
            try
            {
                string URL = ConfigurationManager.AppSettings["URLPromo"];

                HttpClient client = new HttpClient();
                var data = new StringContent(JsonConvert.SerializeObject(promo), Encoding.UTF8, "application/json");

                // List data response.
                HttpResponseMessage response = client.PostAsync(URL, data).Result;

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    using (IBaseDatos baseDatos = BaseDatos.Construir(new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["STR-CNN-LOYALTY"])))
                    {
                        string[] param = new string[] { "Mensaje" };
                        object[] value = new object[1];
                        value[0] = "RETRY: servicio de PROMOCIONES retorno Status code = " + response.StatusCode + " con el siguiente mensaje = " + response.RequestMessage.Content;

                        baseDatos.EjecutarSP("[LOYALTY].[sp_Logs_Insert]", param, value);
                    };
                    return false;
                }
            }
            catch (Exception e) {
                using (IBaseDatos baseDatos = BaseDatos.Construir(new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["STR-CNN-LOYALTY"])))
                {
                    string[] param = new string[] { "Mensaje" };
                    object[] value = new object[1];
                    value[0] = "ERROR: servicio de Promociones no accesible, = "+e.Message;

                    baseDatos.EjecutarSP("[LOYALTY].[sp_Logs_Insert]", param, value);
                };
                return false;
            }
        }

        //void EnviarMailActivacion(string hashValidacion, string apellido, string nombre, string email)
        //{
        //    var mail = new LibMail.MailSender
        //    {
        //        ServidorSMTP = System.Configuration.ConfigurationManager.AppSettings["ServidorSMTP"],
        //        PuertoServidorSMTP = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["PuertoServidorSMTP"]),
        //        RemitenteMail = System.Configuration.ConfigurationManager.AppSettings["RemitenteMail"],
        //        PassRemitente = System.Configuration.ConfigurationManager.AppSettings["PassRemitenteMail"],
        //        NombreRemitente = System.Configuration.ConfigurationManager.AppSettings["NombreRemitenteMail"],
        //        AsuntoMail = "Bienvenido a Diarco+",
        //        CuerpoMail = Utilities.Mail.CrearCuerpoMailValidacion(Server.UrlEncode(hashValidacion), nombre + " " + apellido, System.Configuration.ConfigurationManager.AppSettings["UrlValidacionMail"]),
        //        Destinatario = email
        //    };

        //    mail.SendMail();
        //}
    }


    public class Ciudades
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class Codigos
    {
        public string Codigo { get; set; }
    }

}
