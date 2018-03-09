using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace UsuarioTest.dbConection
{
    public class SqlHelper
    {
        #region Constants
        #endregion

        #region Variables
        //Used to manage concurrency
        private static object _lockObject = new object();

        //Used to store the current connection string
        private static string _currentConnectionString;
        private static int _SqlExecutionTimeOut;
        #endregion

        #region Properties
        /// <summary>
        /// Current Connection String
        /// </summary>
        protected static string CurrentConnectionString
        {
            get
            {
                return _currentConnectionString;
            }
            set
            {
                _currentConnectionString = value;
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Executes a stored procedure based on the received connection string and stored procedure name
        /// and fills a DataTable with the results of the procedure execution.
        /// </summary>
        /// <param name="connection">Connection string used to connect to a database.</param>
        /// <param name="storedProcedureName">The name of the stored procedure to execute.</param>
        /// <param name="parameterValues">List of the parameters necessary to execute the stored procedure.</param>
        /// <returns>A DataTable with the result of the stored procedure execution.</returns>
        public static DataTable GetProcedureResults(SqlConnection connection, string storedProcedureName)
        {
            return GetProcedureResults(connection, storedProcedureName, new List<SqlParameter>());
        }


        /// <summary>
        /// Executes a stored procedure based on the received connection string and stored procedure name
        /// and fills a DataTable with the results of the procedure execution.
        /// </summary>
        /// <param name="connection">Connection string used to connect to a database.</param>
        /// <param name="storedProcedureName">The name of the stored procedure to execute.</param>
        /// <param name="parameterValues">List of the parameters necessary to execute the stored procedure.</param>
        /// <returns>A DataTable with the result of the stored procedure execution.</returns>
        /// 
        public static int GetSqlExecutionTimeOut
        {
            get
            {

                if (System.Configuration.ConfigurationManager.AppSettings["sqlExecutionTimeOut"] == null || !string.IsNullOrEmpty(System.Configuration.ConfigurationSettings.AppSettings["sqlExecutionTimeOut"].ToString())) return 30;
                try
                {
                    _SqlExecutionTimeOut = int.Parse(System.Configuration.ConfigurationManager.AppSettings["sqlExecutionTimeOut"].ToString());
                    if (_SqlExecutionTimeOut <= 0)
                    {
                        return 100;
                    }
                    return _SqlExecutionTimeOut;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
        public static DataTable GetProcedureResults(SqlConnection connection, string storedProcedureName, params object[] parameterValues)
        {

            //Create a new table for containing the result of the stored procedure
            DataTable result = new DataTable();

            //Create connection based on the connection string
            //connection = new SqlConnection(connectionString);

            //Create the command based on the stored procedure name and created connection
            SqlCommand command = new SqlCommand(storedProcedureName);
            //Define the type of command
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = GetSqlExecutionTimeOut;

            //Create adapter for the created command
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            try
            {
                lock (_lockObject)
                {

                    //Open connection
                    ////connection.Open();

                    //Populate the command parameters collection using a static method 
                    //of the SqlCommandBuilder class
                    SqlCommandBuilder.DeriveParameters(command);

                    //Initialize an index for the parameters array
                    int index = 0;

                    //Update the input parameters values based on the values contained in the array
                    foreach (SqlParameter parameter in command.Parameters)
                    {
                        //Validate if it's an input parameter
                        if (parameter.Direction == ParameterDirection.Input ||
                            parameter.Direction == ParameterDirection.InputOutput)
                        {
                            if (parameterValues[index] == null || string.IsNullOrEmpty(parameterValues[index].ToString()))
                            {
                                parameter.Value = DBNull.Value;
                            }
                            else
                            {
                                parameter.Value = parameterValues[index];
                            }
                            index++;
                        }
                    }
                    //Execute the adapter's method the fills the DataTable based on the associated command.
                    adapter.Fill(result);
                }
                //Return the result
                return result;
            }
            catch (Exception ex)
            {
                RollBack(ref command);
                string error = ex.ToString();
                //Throw the exception to upper layers
                throw;
            }
            finally
            {
                Disponse(ref result);
                if (adapter != null)
                {
                    adapter.Dispose();
                    adapter = null;
                }
                if (!(connection == null))
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }
        public static void Disponse(ref DataTable dsDataset)
        {
            if (dsDataset != null)
            {
                dsDataset.Dispose();
                dsDataset = null;
            }
        }
        public static void Disponse(ref DataSet dsDataset)
        {
            if (dsDataset != null)
            {
                dsDataset.Dispose();
                dsDataset = null;
            }
        }
        public static void RollBack(ref SqlCommand sqlcCommand)
        {
            if (sqlcCommand != null)
            {
                sqlcCommand.Transaction.Rollback();
                sqlcCommand.Transaction.Dispose();
                sqlcCommand.Transaction = null;
            }
            Disponse(ref sqlcCommand);
        }

        public static void Disponse(ref SqlCommand cmdSQLcommand)
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
        /// <summary>
        /// Executes a stored procedure based on the received connection string and stored procedure name
        /// and fills a DataTable with the results of the procedure execution.
        /// </summary>
        /// <param name="connection">Connection string used to connect to a database.</param>
        /// <param name="storedProcedureName">The name of the stored procedure to execute.</param>
        /// <param name="parameterValues">List of the parameters necessary to execute the stored procedure.</param>
        /// <returns>A DataTable with the result of the stored procedure execution.</returns>
        /// 
        public static DataSet GetProcedureResultsAndReturnDataSet(SqlConnection connection, string storedProcedureName)
        {
            return GetProcedureResultsAndReturnDataSet(connection, storedProcedureName, new List<SqlParameter>());
        }
        public static DataSet GetProcedureResultsAndReturnDataSet(SqlConnection connection, string storedProcedureName, params object[] parameterValues)
        {

            //Create a new table for containing the result of the stored procedure
            DataSet result = new DataSet();

            //Create connection based on the connection string
            //connection = new SqlConnection(connectionString);

            //Create the command based on the stored procedure name and created connection
            SqlCommand command = new SqlCommand(storedProcedureName);
            //Define the type of command
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = GetSqlExecutionTimeOut;

            //Create adapter for the created command
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            try
            {
                lock (_lockObject)
                {

                    //Open connection
                    ////connection.Open();

                    //Populate the command parameters collection using a static method 
                    //of the SqlCommandBuilder class
                    SqlCommandBuilder.DeriveParameters(command);

                    //Initialize an index for the parameters array
                    int index = 0;

                    //Update the input parameters values based on the values contained in the array
                    foreach (SqlParameter parameter in command.Parameters)
                    {
                        //Validate if it's an input parameter
                        if (parameter.Direction == ParameterDirection.Input ||
                            parameter.Direction == ParameterDirection.InputOutput)
                        {
                            if (parameterValues[index] == null || string.IsNullOrEmpty(parameterValues[index].ToString()))
                            {
                                parameter.Value = DBNull.Value;
                            }
                            else
                            {
                                parameter.Value = parameterValues[index];
                            }
                            index++;
                        }
                    }

                    //Execute the adapter's method the fills the DataTable based on the associated command.
                    adapter.Fill(result);
                }
                //Return the result
                return result;
            }
            catch (Exception ex)
            {
                RollBack(ref command);
                string error = ex.ToString();
                //Throw the exception to upper layers
                throw;
            }
            finally
            {
                Disponse(ref result);
                if (adapter != null)
                {
                    adapter.Dispose();
                    adapter = null;
                }
                if (!(connection == null))
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Executes a stored procedure based on the received connection string and stored procedure name (Used to delete of update)
        /// </summary>
        /// <param name="storedProcedureName">The name of the stored procedure to execute.</param>
        /// <param name="parameterValues">List of the parameters necessary to execute the stored procedure.</param>
        /// <returns>The number of rows affected by the procedure execution.</returns>
        public static int ExecuteProcedure(SqlConnection connection, string storedProcedureName, params object[] parameterValues)
        {

            //Create the command based on the stored procedure name and created connection
            SqlCommand command = new SqlCommand(storedProcedureName, connection);
            //Define the type of command
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            try
            {

                lock (_lockObject)
                {
                    //Open connection
                    connection.Open();

                    //Populate the command parameters collection using a static method of the SqlCommandBuilder class
                    SqlCommandBuilder.DeriveParameters(command);

                    //Initialize an index for the parameters array
                    int index = 0;

                    //Update the input parameters values based on the values contained in the array
                    foreach (SqlParameter parameter in command.Parameters)
                    {
                        //Validate if it's an input parameter
                        if (parameter.Direction == ParameterDirection.Input ||
                            parameter.Direction == ParameterDirection.InputOutput)
                        {
                            if (parameterValues[index] == null || string.IsNullOrEmpty(parameterValues[index].ToString()))
                            {
                                parameter.Value = DBNull.Value;
                            }
                            else
                            {
                                parameter.Value = parameterValues[index];
                            }
                            index++;
                        }
                    }

                    //Returns the number of rows affected
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();

                //Throw the exception to upper layers
                throw;
            }
            finally
            {
                if (!(connection == null))
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Gets Query Result
        /// </summary>
        /// <param name="query">Query</param>
        /// <returns>Returns a Data Table result</returns>
        public static DataTable GetQueryResult(SqlConnection connection, string query)
        {

            //Create a new table for containing the result of the stored procedure
            DataTable result = new DataTable();

            //Create the command based on the stored procedure name and created connection
            SqlCommand command = new SqlCommand(query, connection);
            //Define the type of command
            command.CommandType = CommandType.Text;

            //Create adapter for the created command
            SqlDataAdapter adapter = new SqlDataAdapter(command);

            try
            {

                lock (_lockObject)
                {
                    //Open connection
                    command.Connection.Open();

                    adapter.Fill(result);
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();

                //Throw the exception to upper layers
                throw;
            }
            finally
            {
                if (!(connection == null))
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            return result;
        }

        // <summary>
        /// Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the database specified in 
        /// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
        /// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
        /// </summary>
        /// <remarks>
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// 
        /// e.g.:  
        ///  int orderCount = (int)ExecuteScalar(connString, "storedProcedureName", parameterValues);
        /// </remarks>
        /// <param name="storedProcedureName">The name of the stored procedure</param>
        /// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the command</returns>
        /// 

        public static object ExecuteScalar(SqlConnection connection, string storedProcedureName)
        {
            return ExecuteScalar(connection, storedProcedureName, new List<SqlParameter>());
        }

        public static object ExecuteScalar(SqlConnection connection, string storedProcedureName, params object[] parameterValues)
        {

            //Create the command based on the stored procedure name and created connection
            SqlCommand command = new SqlCommand(storedProcedureName);
            //Define the type of command
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = GetSqlExecutionTimeOut; ;

            try
            {

                lock (_lockObject)
                {

                    //Open connection
                    ////connection.Open();

                    //Populate the command parameters collection using a static method of the SqlCommandBuilder class
                    SqlCommandBuilder.DeriveParameters(command);

                    //Initialize an index for the parameters array
                    int index = 0;

                    //Update the input parameters values based on the values contained in the array
                    foreach (SqlParameter parameter in command.Parameters)
                    {
                        //Validate if it's an input parameter
                        if (parameter.Direction == ParameterDirection.Input ||
                            parameter.Direction == ParameterDirection.InputOutput)
                        {
                            if (parameterValues[index] == null || string.IsNullOrEmpty(parameterValues[index].ToString()))
                            {
                                parameter.Value = DBNull.Value;
                            }
                            else
                            {
                                parameter.Value = parameterValues[index];
                            }
                            index++;
                        }
                    }

                    //Return the number of rows affected
                    return command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                RollBack(ref command);
                string error = ex.ToString();
                //Throw the exception to upper layers
                throw;
            }
            finally
            {
                if (!(connection == null))
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        #endregion
    }
}