using System;
using System.Data;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace ObjetosNegocio.Datos
{
    #region Interfaces

    public interface IBaseDatos : IDisposable
    {
        DateTime MinDateTime { get; }

        bool EnTransaccion { get; }
        void FinalizarTransaccion();
        void CancelarTransaccion();
        void ComenzarTransaccion();

        int Ejecutar(string sentencia);

        void EjecutarConsulta(DataSet datos, MissingSchemaAction accionFaltaEsquema, string consulta, params string[] nombresTablas);
        void EjecutarConsulta(DataTable tabla, MissingSchemaAction accionFaltaEsquema, string consulta);

        void EjecutarSP(DataSet datos, string nombre, string[] nombresParam, object[] valoresParam);
        void EjecutarSP(DataTable datos, string nombre, string[] nombresParam, object[] valoresParam);
        void EjecutarSP(DataTable datos, string nombre);

        object EjecutarSP(string nombre, DataRow registro);
        object EjecutarSP(string nombre, DataRow registro, DataRowVersion version);
        object EjecutarSP(string nombre, DataRow registro, params DataColumn[] parametrosSalida);
        object EjecutarSP(string nombre, string[] nombresParam, object[] valoresParam);
        object[] EjecutarSP(string nombre, string[] nombresParam, ParameterDirection[] direccion, object[] valoresParam);

        void ObtenerTabla(DataTable datos);
        void ObtenerTabla(DataTable datos, string filtro, params object[] parametros);
        void ObtenerTabla(DataTable datos, MissingSchemaAction accionFaltaEsquema, string filtro, params object[] parametros);

        void ObtenerTabla(DataSet datos, string nombreTabla);
        void ObtenerTabla(DataSet datos, string nombreTabla, string filtro, params object[] parametros);
        void ObtenerTabla(DataSet datos, string nombreTabla, MissingSchemaAction accionFaltaEsquema, string filtro, params object[] parametros);

        string Format(DateTime value);
    }

    #endregion

    public abstract class BaseDatos
    {
        #region Ejecución de consultas

        abstract public void EjecutarConsulta(DataSet datos, MissingSchemaAction accionFaltaEsquema, string consulta, params string[] nombresTablas);

        abstract public void EjecutarConsulta(DataTable tabla, MissingSchemaAction accionFaltaEsquema, string consulta);

        abstract public int Ejecutar(string sentencia);

        #endregion

        #region Metodos de obtención de datos

        public void ObtenerTabla(DataSet datos, string nombreTabla)
        {
            ObtenerTabla(datos, nombreTabla, MissingSchemaAction.Error, string.Empty);
        }

        public void ObtenerTabla(DataSet datos, string nombreTabla, string filtro, params object[] parametros)
        {
            ObtenerTabla(datos, nombreTabla, MissingSchemaAction.Error, filtro, parametros);
        }

        public void ObtenerTabla(DataSet datos, string nombreTabla, MissingSchemaAction accionFaltaEsquema, string filtro, params object[] parametros)
        {
            if (string.IsNullOrEmpty(filtro))
                EjecutarConsulta(datos, MissingSchemaAction.Error, string.Format("SELECT * FROM \"{0}\"", nombreTabla), nombreTabla);
            else
                EjecutarConsulta(datos, accionFaltaEsquema, string.Format("SELECT * FROM \"{0}\" WHERE {1}", nombreTabla, string.Format(filtro, parametros)), nombreTabla);
        }


        public void ObtenerTabla(DataTable datos)
        {
            ObtenerTabla(datos, MissingSchemaAction.Error, string.Empty);
        }

        public void ObtenerTabla(DataTable datos, string filtro, params object[] parametros)
        {
            ObtenerTabla(datos, MissingSchemaAction.Error, filtro, parametros);
        }

        public void ObtenerTabla(DataTable datos, MissingSchemaAction accionFaltaEsquema, string filtro, params object[] parametros)
        {
            if (string.IsNullOrEmpty(filtro))
                EjecutarConsulta(datos, accionFaltaEsquema, string.Format("SELECT * FROM \"{0}\"", datos.TableName));
            else
                EjecutarConsulta(datos, accionFaltaEsquema, string.Format("SELECT * FROM \"{0}\" WHERE {1}", datos.TableName, string.Format(filtro, parametros)));
        }

        #endregion

        #region Formatos

        public virtual DateTime MinDateTime
        {
            get
            {
                return DateTime.MinValue;
            }
        }

        public virtual string Format(DateTime value)
        {
            return string.Format("'{0:yyyy-MM-dd 00:00:00}'", value);
        }

        public virtual string Format(string value)
        {
            return string.Format("'{0}'", value);
        }

        public virtual string Format(int value)
        {
            return value.ToString();
        }

        public virtual string Format(decimal value)
        {
            return value.ToString();
        }

        public virtual string Format(double value)
        {
            return value.ToString();
        }

        public virtual string Format(float value)
        {
            return value.ToString();
        }

        #endregion

        #region Constructor/Factory

        public static IBaseDatos Construir()
        {
            return Construir("default");
        }

        public static IBaseDatos Construir(string tipoBaseDatos)
        {
            string proveedor = ConfigurationManager.ConnectionStrings[tipoBaseDatos].ProviderName;

            switch (proveedor.ToLower())
            {
                case "sqlserver":
                    return new BaseDatosSqlServer(tipoBaseDatos);
            }

            throw new NotImplementedException();
        }

        public static IBaseDatos Construir(SqlConnection connection)
        {
            return new BaseDatosSqlServer(connection);
        }

        #endregion
    }

    public class BaseDatosSqlServer : BaseDatos, IDisposable, IBaseDatos
    {
        private const string SQL_ROWGUID_COLUMN = "rowguid";

        #region Miembros de la instancia

        private SqlConnection _conexion;
        private SqlTransaction _transaccion;

        #endregion

        #region Propiedades

        /// <summary>
        /// Indica si la base de datos se encuentra en trasacción.
        /// </summary>
        public bool EnTransaccion
        {
            get { return _transaccion != null; }
        }

        #endregion

        #region Constructor

        public BaseDatosSqlServer(SqlConnection conexion)
        {
            _conexion = conexion;
            _conexion.Open();
        }

        public BaseDatosSqlServer()
            : this("default")
        {

        }

        public BaseDatosSqlServer(string tipoBaseDatos)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings[tipoBaseDatos].ConnectionString;

            _conexion = new SqlConnection(cadenaConexion);

            _conexion.Open();
        }

        #endregion

        #region Procedimientos de control de transacciones

        public void ComenzarTransaccion()
        {
            _transaccion = _conexion.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void FinalizarTransaccion()
        {
            _transaccion.Commit();

            _transaccion = null;
        }

        public void CancelarTransaccion()
        {
            _transaccion.Rollback();

            _transaccion = null;
        }

        #endregion

        #region Ejecución de SP

        private string FormatoNombre(string nombreProcedimiento)
        {
            return nombreProcedimiento;
        }

        public object EjecutarSP(string nombre, DataRow registro)
        {
            return EjecutarSP(nombre, registro, DataRowVersion.Default);
        }

        public object EjecutarSP(string nombre, DataRow registro, DataRowVersion version)
        {
            try
            {
                DataColumnCollection columnas = registro.Table.Columns;

                using (SqlCommand cmd = new SqlCommand(FormatoNombre(nombre), _conexion))
                {
                    if (_transaccion != null)
                        cmd.Transaction = _transaccion;

                    cmd.CommandType = CommandType.StoredProcedure;

                    for (int i = 0; i < columnas.Count; i++)
                    {
                        if (string.IsNullOrEmpty(columnas[i].Expression) && columnas[i].ColumnName != SQL_ROWGUID_COLUMN)
                            AgregarParametro(cmd, ParameterDirection.Input, columnas[i].ColumnName, registro[i, version], registro.Table.Columns[i].DataType);
                    }

                    return cmd.ExecuteScalar();
                }
            }
            catch (SqlException ex)
            {
                throw NuevaExcepcion(ex);
            }
        }

        public object EjecutarSP(string nombre, DataRow registro, params DataColumn[] parametrosSalida)
        {
            try
            {
                DataColumnCollection columnas = registro.Table.Columns;

                using (SqlCommand cmd = new SqlCommand(FormatoNombre(nombre), _conexion))
                {
                    if (_transaccion != null)
                        cmd.Transaction = _transaccion;

                    cmd.CommandType = CommandType.StoredProcedure;

                    for (int i = 0; i < columnas.Count; i++)
                    {
                        if (string.IsNullOrEmpty(columnas[i].Expression) && columnas[i].ColumnName != SQL_ROWGUID_COLUMN)
                            AgregarParametro(cmd, ParameterDirection.Input, columnas[i].ColumnName, registro[i], registro.Table.Columns[i].DataType);
                    }

                    foreach (DataColumn parametroSalida in parametrosSalida)
                    {
                        if (parametroSalida.DataType != typeof(byte[]))
                            cmd.Parameters[string.Format("@{0}", parametroSalida.ColumnName)].Direction = ParameterDirection.InputOutput;
                    }

                    cmd.ExecuteScalar();

                    foreach (DataColumn parameterSalida in parametrosSalida)
                        registro[parameterSalida.ColumnName] = cmd.Parameters[string.Format("@{0}", parameterSalida.ColumnName)].Value;

                    return parametrosSalida.Length > 0 ? cmd.Parameters[string.Format("@{0}", parametrosSalida[0].ColumnName)].Value : null;
                }
            }
            catch (SqlException ex)
            {
                throw NuevaExcepcion(ex);
            }
        }

        public object EjecutarSP(string nombre, string[] nombresParam, object[] valoresParam)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(FormatoNombre(nombre), _conexion))
                {
                    if (_transaccion != null)
                        cmd.Transaction = _transaccion;

                    cmd.CommandType = CommandType.StoredProcedure;

                    for (int i = 0; i < nombresParam.Length; i++)
                        AgregarParametro(cmd, ParameterDirection.Input, nombresParam[i], valoresParam[i]);

                    return cmd.ExecuteScalar();
                }
            }
            catch (SqlException ex)
            {
                throw NuevaExcepcion(ex);
            }
        }

        public object[] EjecutarSP(string nombre, string[] nombresParam, ParameterDirection[] direccion, object[] valoresParam)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(FormatoNombre(nombre), _conexion))
                {
                    if (_transaccion != null)
                        cmd.Transaction = _transaccion;

                    cmd.CommandType = CommandType.StoredProcedure;

                    int cantSalidas = 0;

                    for (int i = 0; i < nombresParam.Length; i++)
                    {
                        AgregarParametro(cmd, direccion[i], nombresParam[i], valoresParam[i]);

                        if (direccion[i] != ParameterDirection.Input)
                            cantSalidas++;
                    }

                    cmd.ExecuteScalar();

                    object[] ret = new object[cantSalidas];

                    int j = 0;

                    foreach (SqlParameter parametro in cmd.Parameters)
                    {
                        if (parametro.Direction != ParameterDirection.Input)
                        {
                            switch (parametro.SqlDbType)
                            {
                                case SqlDbType.DateTime:
                                    ret[j++] = (DateTime)parametro.Value;
                                    break;
                                case SqlDbType.Decimal:
                                    ret[j++] = (decimal)parametro.Value;
                                    break;
                                default:
                                    ret[j++] = parametro.Value;
                                    break;
                            }
                        }
                    }

                    return ret;
                }
            }
            catch (SqlException ex)
            {
                throw NuevaExcepcion(ex);
            }
        }

        public void EjecutarSP(DataSet datos, string nombre, string[] nombresParam, object[] valoresParam)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(nombre, _conexion))
                {
                    if (_transaccion != null)
                        cmd.Transaction = _transaccion;

                    cmd.CommandType = CommandType.StoredProcedure;

                    for (int i = 0; i < nombresParam.Length; i++)
                        AgregarParametro(cmd, ParameterDirection.Input, nombresParam[i], valoresParam[i]);

                    using (SqlDataAdapter adaptador = new SqlDataAdapter(cmd))
                    {
                        adaptador.Fill(datos);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw NuevaExcepcion(ex);
            }
        }

        public void EjecutarSP(DataTable datos, string nombre, string[] nombresParam, object[] valoresParam)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(nombre, _conexion))
                {
                    if (_transaccion != null)
                        cmd.Transaction = _transaccion;

                    cmd.CommandType = CommandType.StoredProcedure;

                    for (int i = 0; i < nombresParam.Length; i++)
                        AgregarParametro(cmd, ParameterDirection.Input, nombresParam[i], valoresParam[i]);

                    using (SqlDataAdapter adaptador = new SqlDataAdapter(cmd))
                    {
                        adaptador.Fill(datos);
                    }

                    if (string.IsNullOrEmpty(datos.TableName))
                        datos.TableName = nombre;
                }
            }
            catch (SqlException ex)
            {
                throw NuevaExcepcion(ex);
            }
        }

        public void EjecutarSP(DataTable tabla, string nombre)
        {
            EjecutarSP(tabla, nombre, new string[] { }, new object[] { });
        }

        private void AgregarParametro(SqlCommand cmd, ParameterDirection direccion, string nombre, object valor)
        {
            if (valor != null)
                AgregarParametro(cmd, direccion, nombre, valor, valor.GetType());
            else
                AgregarParametro(cmd, direccion, nombre, valor, typeof(object));
        }

        private void AgregarParametro(SqlCommand cmd, ParameterDirection direccion, string nombre, object valor, Type tipo)
        {
            SqlParameter parametro = new SqlParameter();

            parametro.Direction = direccion;
            parametro.ParameterName = string.Format("@{0}", nombre);
            parametro.Value = valor != null ? valor : DBNull.Value;

            if (tipo == typeof(byte[]))
                parametro.SqlDbType = SqlDbType.Image;
            else if (tipo == typeof(string))
                parametro.SqlDbType = SqlDbType.VarChar;

            cmd.Parameters.Add(parametro);
        }

        #endregion

        #region Ejecución de consultas

        public override void EjecutarConsulta(DataSet datos, MissingSchemaAction accionFaltaEsquema, string consulta, params string[] nombresTablas)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(consulta, _conexion))
                {
                    if (_transaccion != null)
                        cmd.Transaction = _transaccion;

                    cmd.CommandType = CommandType.Text;

                    using (SqlDataAdapter adaptador = new SqlDataAdapter(cmd))
                    {
                        datos.Locale = System.Globalization.CultureInfo.CurrentCulture;

                        adaptador.MissingSchemaAction = accionFaltaEsquema;

                        for (int i = 0; i < nombresTablas.Length; i++)
                            adaptador.Fill(datos, nombresTablas[i]);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw NuevaExcepcion(ex);
            }
        }

        public override void EjecutarConsulta(DataTable tabla, MissingSchemaAction accionFaltaEsquema, string consulta)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(consulta, _conexion))
                {
                    if (_transaccion != null)
                        cmd.Transaction = _transaccion;

                    cmd.CommandType = CommandType.Text;

                    using (SqlDataAdapter adaptador = new SqlDataAdapter(cmd))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            ds.Locale = System.Globalization.CultureInfo.CurrentCulture;

                            adaptador.Fill(ds);

                            tabla.Merge(ds.Tables[0], false, accionFaltaEsquema);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw NuevaExcepcion(ex);
            }
        }

        public override int Ejecutar(string sentencia)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(sentencia, _conexion))
                {
                    if (_transaccion != null)
                        cmd.Transaction = _transaccion;

                    cmd.CommandType = CommandType.Text;

                    return cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw NuevaExcepcion(ex);
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_conexion.State != ConnectionState.Closed)
                _conexion.Close();

            _conexion.Dispose();
        }

        #endregion

        #region Formatos

        public override DateTime MinDateTime
        {
            get
            {
                return new DateTime(1755, 1, 1);
            }
        }

        #endregion

        #region Excepciones

        private Exception NuevaExcepcion(SqlException ex)
        {
            //Violación de restrcción
            switch (ex.Number)
            {
                case 547:
                case 1767:
                case 1768:
                case 1769:
                case 1770:
                case 2601:
                case 2627:
                case 3725:
                case 3726:
                case 10055:
                case 10065:
                case 11011:
                case 11040:
                case 15470:
                    return new ExcepcionBaseDatosRestriccion(ex.Message, ex);
                default:
                    return ex;
            }
        }

        #endregion
    }

    public class ExcepcionBaseDatosRestriccion : ApplicationException
    {
        public ExcepcionBaseDatosRestriccion(string mensaje, Exception ex) : base(mensaje, ex) { }
    }

}


