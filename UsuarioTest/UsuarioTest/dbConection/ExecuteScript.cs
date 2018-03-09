using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace UsuarioTest.dbConection
{
    public class ExecuteScript: DatabaseEngine 
    {
        public void executeQuertyAndReturnNothing(string storedProcedureName, params object[] parameterValues)
        {
            try
            {
                SqlHelper.ExecuteScalar(this.getTheConnectionString, storedProcedureName, parameterValues);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void executeQuertyAndReturnNothing(string storedProcedureName)
        {
            try
            {
                SqlHelper.ExecuteScalar(this.getTheConnectionString, storedProcedureName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int executeQuertyAndReturnInteger(string storedProcedureName, params object[] parameterValues)
        {
            try
            {
                object strResult = SqlHelper.ExecuteScalar(this.GetConnection(), storedProcedureName, parameterValues);
                if (strResult == null || strResult.ToString() == string.Empty) return -1;
                if (!IsNumeric(strResult.ToString())) return -1;
                return int.Parse(strResult.ToString());

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int executeQuertyAndReturnInteger(string storedProcedureName)
        {
            try
            {
                object strResult = SqlHelper.ExecuteScalar(this.GetConnection(), storedProcedureName);
                if (strResult == null || strResult.ToString() == string.Empty) return -1;
                if (!IsNumeric(strResult.ToString())) return -1;
                return int.Parse(strResult.ToString());

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool executeQuertyAndReturnBool(string storedProcedureName, params object[] parameterValues)
        {
            try
            {
                object strResult = SqlHelper.ExecuteScalar(this.GetConnection(), storedProcedureName, parameterValues);
                if (strResult == null || strResult.ToString() == string.Empty) return false;
                if (!IsNumeric(strResult.ToString())) return false;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool executeQuertyAndReturnBool(string storedProcedureName)
        {
            try
            {
                object strResult = SqlHelper.ExecuteScalar(this.GetConnection(), storedProcedureName);
                if (strResult == null || strResult.ToString() == string.Empty) return false;
                if (!IsNumeric(strResult.ToString())) return false;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string executeQuertyAndReturnString(string storedProcedureName, params object[] parameterValues)
        {
            try
            {
                object strResult = SqlHelper.ExecuteScalar(this.GetConnection(), storedProcedureName, parameterValues);
                if (strResult == null || strResult.ToString() == string.Empty) return string.Empty;
                return strResult.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string executeQuertyAndReturnString(string storedProcedureName)
        {
            try
            {
                object strResult = SqlHelper.ExecuteScalar(this.GetConnection(), storedProcedureName);
                if (strResult == null || strResult.ToString() == string.Empty) return string.Empty;
                return strResult.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public object executeQuertyAndReturnObject(string storedProcedureName, params object[] parameterValues)
        {
            try
            {
                object strResult = SqlHelper.ExecuteScalar(this.GetConnection(), storedProcedureName, parameterValues);
                return strResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public object executeQuertyAndReturnObject(string storedProcedureName)
        {
            try
            {
                object strResult = SqlHelper.ExecuteScalar(this.GetConnection(), storedProcedureName);
                return strResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable executeQuertyAndReturnDatatable(string storedProcedureName, params object[] parameterValues)
        {
            DataTable dt = null;
            try
            {
                dt = SqlHelper.GetProcedureResults(this.GetConnection(), storedProcedureName, parameterValues);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        public DataTable executeQuertyAndReturnDatatable(string storedProcedureName)
        {
            DataTable dt = null;
            try
            {
                dt = SqlHelper.GetProcedureResults(this.GetConnection(), storedProcedureName);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        public DataSet executeQuertyAndReturnDataSet(string storedProcedureName, params object[] parameterValues)
        {
            DataSet ds = null;
            try
            {
                ds = SqlHelper.GetProcedureResultsAndReturnDataSet(this.GetConnection(), storedProcedureName, parameterValues);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (ds != null) ds.Dispose();
                ds = null;
            }
        }

        public DataSet executeQuertyAndReturnDataSet(string storedProcedureName)
        {
            DataSet ds = null;
            try
            {
                ds = SqlHelper.GetProcedureResultsAndReturnDataSet(this.GetConnection(), storedProcedureName);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (ds != null) ds.Dispose();
                ds = null;
            }
        }
    }
}