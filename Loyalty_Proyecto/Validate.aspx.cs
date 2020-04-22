using ObjetosNegocio.Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGM_LOYALTY
{
    public partial class Validate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var token = Request.QueryString["c"];
                if (string.IsNullOrEmpty(token))
                {

                }
                else
                {
                    Validar(token);
                }

            }
        }
        void Validar(string token)
        {
            //busco en la tabla clientes quien tiene el token pasado como parámetro
            string[] param = new string[] { "HashValidacion"};
            object[] value = new object[1];
            DataTable resultado = new DataTable();
            var mensaje = string.Empty;

            value[0] = token;

            using (IBaseDatos baseDatos = BaseDatos.Construir(new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["STR-CNN-LOYALTY"])))
            {
                baseDatos.EjecutarSP(resultado, "[LOYALTY].[sp_Clientes_Activar]", param, value);

                if (resultado.Rows.Count == 0)
                {
                    mensaje = "No se pudo activar el cliente. Reintente más tarde.";
                }
                else
                {
                    if (resultado.Rows[0]["Codigo"].ToString() == "SI")
                    {
                        if (resultado.Rows[0]["Genero"].ToString() == "F")
                            mensaje = "Bienvenida, " + resultado.Rows[0]["Apellido"].ToString() + " " + resultado.Rows[0]["Nombre"].ToString() + "<br/>Su tarjeta Diarco+ ha sido habilitada y ya puede utilizar sus beneficios.";
                        else
                            mensaje = "Bienvenido, " + resultado.Rows[0]["Apellido"].ToString() + " " + resultado.Rows[0]["Nombre"].ToString() + "<br/>Su tarjeta Diarco+ ha sido habilitada y ya puede utilizar sus beneficios.";

                        try
                        {
                            //EnviarMailActivacionRealizada(resultado.Rows[0]["Apellido"].ToString(), resultado.Rows[0]["Nombre"].ToString(), resultado.Rows[0]["Genero"].ToString(), resultado.Rows[0]["Tarjeta"].ToString(), resultado.Rows[0]["Email"].ToString());
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else
                    {
                        mensaje = "No se pudo activar el cliente. " + resultado.Rows[0]["Mensaje"].ToString();
                    }
                }

                lblMensajeActivacion.Text = mensaje;
            }
        }
        //void EnviarMailActivacionRealizada(string apellido, string nombre, string genero, string tarjeta, string email)
        //{
        //    var mail = new LibMail.MailSender
        //    {
        //        ServidorSMTP = System.Configuration.ConfigurationManager.AppSettings["ServidorSMTP"],
        //        PuertoServidorSMTP = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["PuertoServidorSMTP"]),
        //        RemitenteMail = System.Configuration.ConfigurationManager.AppSettings["RemitenteMail"],
        //        PassRemitente = System.Configuration.ConfigurationManager.AppSettings["PassRemitenteMail"],
        //        NombreRemitente = System.Configuration.ConfigurationManager.AppSettings["NombreRemitenteMail"],
        //        AsuntoMail = "Tarjeta Diarco+ activada",
        //        CuerpoMail = Utilities.Mail.CrearCuerpoMailValidacionRealizada(nombre + " " + apellido, genero, tarjeta),
        //        Destinatario = email
        //    };

        //    mail.SendMail();
        //}
    }
}