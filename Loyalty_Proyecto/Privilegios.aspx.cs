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


namespace SGM_LOYALTY
{
    public partial class Privilegios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (! this.IsPostBack)
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
            string[] param = new string[] { "DNI", "Tarjeta" };
            object[] value = new object[2];
            value[0] = this.hfDNI.Value.ToString();
            value[1] = this.hfTarjeta.Value.ToString();

            DataTable resultado = new DataTable();
            try
            {
                using (IBaseDatos baseDatos = BaseDatos.Construir(new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["STR-CNN-LOYALTY"])))
                {
                    baseDatos.EjecutarSP(resultado, "[LOYALTY].[sp_Empleados_Insert]", param, value);

                    foreach (DataRow item in resultado.Rows)
                    {
                        if (item[0].ToString() == "SI")
                        {
                            hfOperacion.Value = "ok";
                        }
                        else
                        {
                            this.lblError.Text = item[1].ToString();
                            hfOperacion.Value = "error";

                        }
                    }
                    return;
                }
                
            }
            catch (Exception ex)
            {
                hfOperacion.Value = "error";
            }

        }

    }

}