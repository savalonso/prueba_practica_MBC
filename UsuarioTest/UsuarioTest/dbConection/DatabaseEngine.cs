using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace UsuarioTest.dbConection
{
    public class DatabaseEngine
    {

        #region "VAREABLES"
        private static string _strConnectionStirng;
        private static int _SqlExecutionTimeOut;
        public static string _strServerConnectionString;
        #endregion

        #region "CONSTRUCTORS"

        public DatabaseEngine()
        {
            string strIDConection = string.Empty;
            _strConnectionStirng = string.Empty;
            _SqlExecutionTimeOut = 0;
            _strServerConnectionString = string.Empty;
        }

        #endregion

        #region "PROPERTY"
        public string getTheCurrentConnectionString
        {
            get
            {
                if (_strServerConnectionString != string.Empty && !_strServerConnectionString.Equals(string.Empty))
                {
                    _strConnectionStirng = _strServerConnectionString;
                }
                if (_strConnectionStirng == null || _strConnectionStirng == string.Empty)
                {
                    if (System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"] == null) return string.Empty;
                    _strConnectionStirng = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                    if (_strConnectionStirng == string.Empty) return string.Empty;
                }
                return _strConnectionStirng;
            }
        }
        public int GetSqlExecutionTimeOut
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["sqlExecutionTimeOut"] == null) return 30;
                try
                {
                    _SqlExecutionTimeOut = int.Parse(System.Configuration.ConfigurationManager.AppSettings["sqlExecutionTimeOut"].ToString());
                    if (_SqlExecutionTimeOut <= 0)
                    {
                        return 0;
                    }
                    return _SqlExecutionTimeOut;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
        public SqlConnection GetConnection()
        {
            SqlConnection _oConnection = new SqlConnection(this.getTheCurrentConnectionString);
            try
            {
                if (_oConnection != null)
                {
                    switch (_oConnection.State)
                    {
                        case ConnectionState.Connecting:
                        case ConnectionState.Executing:
                        case ConnectionState.Fetching:
                        case ConnectionState.Closed:
                        case ConnectionState.Broken:
                            _oConnection.Open();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    _oConnection = new SqlConnection(this.getTheCurrentConnectionString);
                    _oConnection.Open();
                }
                return _oConnection;

            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool GetConnectionWindowAutentification()
        {
            SqlConnection _oConnection = new SqlConnection();
            SqlCommand sqlcCommand = new SqlCommand();
            bool bIsValue = false;
            try
            {
                _oConnection.ConnectionString = this.getTheCurrentConnectionString;
                sqlcCommand = new SqlCommand(_oConnection.ToString());
                sqlcCommand.CommandTimeout = 2000;
                if (_oConnection != null)
                {
                    switch (_oConnection.State)
                    {
                        case ConnectionState.Connecting:
                        case ConnectionState.Executing:
                        case ConnectionState.Fetching:
                        case ConnectionState.Closed:
                        case ConnectionState.Broken:
                            _oConnection.Open();
                            bIsValue = true;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    _oConnection = new SqlConnection(this.getTheCurrentConnectionString);
                    _oConnection.Open();
                    bIsValue = true;
                }
                return bIsValue;

            }
            catch (System.Exception)
            {
                return false;
            }
            finally
            {
                if (sqlcCommand != null)
                {
                    sqlcCommand.Dispose();
                }
                if (_oConnection != null)
                {
                    _oConnection.Close();
                    _oConnection.Dispose();
                }
            }
        }
        #endregion

        #region "DESTRUCTORES"
        public void Disponse(Boolean disposing, System.Data.SqlClient.SqlCommand cmdSQLcommand)
        {
            try
            {
                if (cmdSQLcommand != null)
                {
                    if (cmdSQLcommand.Transaction != null)
                    {
                        cmdSQLcommand.Transaction.Dispose();
                        cmdSQLcommand.Transaction = null;
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cmdSQLcommand.Dispose();
                cmdSQLcommand = null;
            }
        }
        public void Disponse(ref SqlCommand cmdSQLcommand)
        {
            //DISPONSE THE TRANSACTION
            try
            {
                if (cmdSQLcommand != null)
                {
                    if (cmdSQLcommand.Transaction != null)
                    {
                        cmdSQLcommand.Transaction.Dispose();
                        cmdSQLcommand.Transaction = null;
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cmdSQLcommand.Dispose();
                cmdSQLcommand = null;
            }
        }
        public void Disponse(ref DataSet dsDataset)
        {
            if (dsDataset != null)
            {
                dsDataset.Dispose();
                dsDataset = null;
            }
        }
        protected void RollBack(ref SqlCommand sqlcCommand)
        {
            if (sqlcCommand != null)
            {
                sqlcCommand.Transaction.Rollback();
                sqlcCommand.Transaction.Dispose();
                sqlcCommand.Transaction = null;
            }
            this.Disponse(ref sqlcCommand);
        }
        #endregion

        #region "PROPERTY OF METHOD"

        protected SqlConnection getTheConnectionString
        {
            get
            {
                SqlConnection oConnection = new SqlConnection(this.getTheCurrentConnectionString);
                return oConnection;
            }
        }

        #endregion

        #region  "CONNECCTION TESTING"

        public SqlConnection GetConnectionTestingV2(string strConnectionString)
        {
            SqlConnection _oConnection = new SqlConnection(strConnectionString);
            try
            {
                if (_oConnection != null)
                {
                    switch (_oConnection.State)
                    {
                        case ConnectionState.Connecting:
                        case ConnectionState.Executing:
                        case ConnectionState.Fetching:
                        case ConnectionState.Closed:
                        case ConnectionState.Broken:
                            //_oConnection = new SqlConnection(this.getTheCurrentConnectionString);
                            _oConnection.Open();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    _oConnection = new SqlConnection(this.getTheCurrentConnectionString);
                    _oConnection.Open();
                }
                return _oConnection;

            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool GetConnectionTesting(string strCurrentConnectionString)
        {
            SqlConnection _oConnection = new SqlConnection(strCurrentConnectionString);
            try
            {
                if (_oConnection != null)
                {
                    switch (_oConnection.State)
                    {
                        case ConnectionState.Connecting:
                        case ConnectionState.Executing:
                        case ConnectionState.Fetching:
                        case ConnectionState.Closed:
                        case ConnectionState.Broken:
                            _oConnection = new SqlConnection(strCurrentConnectionString);
                            _oConnection.Open();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    _oConnection = new SqlConnection(strCurrentConnectionString);
                    _oConnection.Open();
                }
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region "VALIDATE NUMBER"
        public bool IsNumeric(string strValue)
        {
            try
            {
                int i = 0;
                bool result = int.TryParse(strValue, out i);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public SqlConnection GetConnectionWithConnectionScript(string strCurrentConnectionString)
        {
            SqlConnection _oConnection = new SqlConnection(strCurrentConnectionString);
            try
            {
                if (_oConnection != null)
                {
                    switch (_oConnection.State)
                    {
                        case ConnectionState.Connecting:
                        case ConnectionState.Executing:
                        case ConnectionState.Fetching:
                        case ConnectionState.Closed:
                        case ConnectionState.Broken:
                            _oConnection.Open();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    _oConnection = new SqlConnection(strCurrentConnectionString);
                    _oConnection.Open();
                }
                return _oConnection;

            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}