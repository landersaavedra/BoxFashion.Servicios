using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql.PostgresTypes;
using System.Data;
using Npgsql.Json;
using Npgsql;
using System.Collections;

namespace BoxFashion.Core.Connection
{
    public class PostGresDataProvider: IDisposible
    {

        private NpgsqlConnection _connection;
        private NpgsqlTransaction _transaction;
        private string _connectionString;
        private string _statementBeforeCloseConnection;
        private string _statementAfterOpenConnection;
        private int _commandTimeout = -1;
        private const string APPLICATION_NAME = "PostGresDataProvider";

        private bool _disposed;

        public string ConnectionString
        {
            get { return this._connectionString; }
            set { this._connectionString = value; }
        }

        public NpgsqlTransaction Transaction
        {
            get
            {
                return this._transaction;
            }
        }

        public string StatementBeforeCloseConnection
        {
            get
            {
                return this._statementBeforeCloseConnection;
            }
            set
            {
                this._statementBeforeCloseConnection = value;
            }
        }

        public string StatementAfterOpenConnection
        {
            get
            {
                return this._statementAfterOpenConnection;
            }
            set
            {
                this._statementAfterOpenConnection = value;
            }
        }

        public NpgsqlConnection GetConnection
        {

            get
            {
                if (this._connection == null || this._connection.State != ConnectionState.Open)
                    this._connection = this.OpenConnection();
                return this._connection;

            }
        }

        public int CommandTimeout
        {
            get
            {
                return this._commandTimeout;
            }
            set
            {
                this._commandTimeout = value;
            }
        }



        public PostGresDataProvider(string pConnectionString)
        {
            this._connectionString = pConnectionString;
        }

        ~PostGresDataProvider()
        {
            this.Dispose(false);
        }

        public NpgsqlConnection OpenConnection()
        {
            try
            {
                return this.OpenConnection(true);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        private NpgsqlConnection OpenConnection(bool LeaveConnectionOpened)
        {
            NpgsqlConnection Connection1 = (NpgsqlConnection)null;
            NpgsqlCommand Command = (NpgsqlCommand)null;
            NpgsqlConnection Connection2;

            try
            {
                Connection1 = new NpgsqlConnection();
                Connection1.ConnectionString = this._connectionString;
                Connection1.Open();

                if (this._statementAfterOpenConnection != null)
                {
                    if (this._statementAfterOpenConnection.Trim().Length > 0)
                    {
                        try
                        {
                            Command = this.CreateCommand(this._statementAfterOpenConnection, new object[0]);
                            Command.Connection = Connection1;
                            Command.ExecuteNonQuery();
                        }
                        finally
                        {
                            if (Command != null)
                                Command.Dispose();

                        }
                    }
                }
                return Connection1;
            }
            catch (NpgsqlException ex)
            {
                if (Connection1 != null)
                {
                    Connection1.Dispose();
                    Connection2 = (NpgsqlConnection)null;
                }
                throw ex;
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (this._connection == null)
                {
                    return;
                }
                if (this._connection.State == ConnectionState.Open)
                    this._connection.Close();
                this._connection.Dispose();
                this._connection = (NpgsqlConnection)null;


            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
        }

        public bool BeginTran()
        {
            try
            {
                if (this._transaction != null || this._connection == null)
                    return false;
                this._transaction = this._connection.BeginTransaction(IsolationLevel.ReadUncommitted);
                return true;
            }

            catch (System.Exception ex)
            {
                if (this._connection != null)
                {
                    this._connection.Dispose();
                    this._connection = (NpgsqlConnection)null;
                }
                if (this._transaction != null)
                    this._transaction = (NpgsqlTransaction)null;
                throw ex;
            }

        }

        public bool CommitTran()
        {
            try
            {
                if (this._transaction == null)
                    return false;
                this._transaction.Commit();
                return true;
            }
            finally
            {
                if (this._transaction != null)
                    this._transaction = (NpgsqlTransaction)null;
            }
        }

        public bool RollBackTran()
        {
            try
            {
                if (this._transaction == null)
                    return false;
                this._transaction.Rollback();
                return true;
            }
            finally
            {
                if (this._transaction != null)
                    this._transaction = (NpgsqlTransaction)null;
            }
        }

        public int ExecuteCommand(string pSql)
        {
            try
            {
                return this.ExecuteCommand(pSql, new object[0]);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public int ExecuteCommand(string pSql, Hashtable pParameters)
        {
            NpgsqlCommand pCommandToExecute = (NpgsqlCommand)null;
            try
            {
                pCommandToExecute = this.CreateCommand(pSql, pParameters);
                return this.ExecuteCommand(pCommandToExecute);
            }
            finally
            {
                if (pCommandToExecute != null)
                    pCommandToExecute.Dispose();
            }
        }

        private int ExecuteCommand(string pSql, object[] pParameters)
        {
            NpgsqlCommand pCommandToExecute = (NpgsqlCommand)null;
            try
            {
                pCommandToExecute = this.CreateCommand(pSql, pParameters);
                return this.ExecuteCommand(pCommandToExecute);
            }
            finally
            {
                if (pCommandToExecute != null)
                    pCommandToExecute.Dispose();
            }
        }

        public int ExecuteCommand(NpgsqlCommand pCommandToExecute)
        {
            try
            {
                pCommandToExecute.Connection = this.GetConnection;
                pCommandToExecute.Transaction = this.Transaction;
                if (this._commandTimeout > -1)
                    pCommandToExecute.CommandTimeout = this._commandTimeout;
                int num = pCommandToExecute.ExecuteNonQuery();
                return num;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public object ExecuteScalar(string pSql)
        {
            try
            {
                return this.ExecuteScalar(pSql, new object[0]);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        private object ExecuteScalar(string pQueryToExecute, object[] pParameters)
        {
            NpgsqlCommand pCommandToExecute = (NpgsqlCommand)null;
            try
            {
                pCommandToExecute = this.CreateCommand(pQueryToExecute, pParameters);
                return this.ExecuteScalar(pCommandToExecute);
            }
            finally
            {
                if (pCommandToExecute != null)
                    pCommandToExecute.Dispose();
            }
        }

        public object ExecuteScalar(string pQueryToExecute, Hashtable pParameters)
        {
            NpgsqlCommand pCommandToExecute = (NpgsqlCommand)null;
            try
            {
                pCommandToExecute = this.CreateCommand(pQueryToExecute, pParameters);
                return this.ExecuteScalar(pCommandToExecute);
            }
            finally
            {
                if (pCommandToExecute != null)
                    pCommandToExecute.Dispose();
            }
        }

        public object ExecuteScalar(NpgsqlCommand pCommandToExecute)
        {
            try
            {
                pCommandToExecute.Connection = this.GetConnection;
                pCommandToExecute.Transaction = this.Transaction;
                if (this._commandTimeout > -1)
                    pCommandToExecute.CommandTimeout = this._commandTimeout;
                object obj = pCommandToExecute.ExecuteScalar();
                return obj;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public DataSet RecordSet(NpgsqlCommand pCommandToExecute, string pTableToCreate, DataSet pDataSetToFill)
        {
            DataSet dataSet = (DataSet)null;
            NpgsqlDataAdapter sqlDataAdapter = (NpgsqlDataAdapter)null;
            NpgsqlConnection sqlConnection = (NpgsqlConnection)null;
            try
            {
                if (pCommandToExecute == null)
                    return (DataSet)null;
                dataSet = pDataSetToFill != null ? pDataSetToFill : new DataSet();
                if (this._commandTimeout >= 0)
                    pCommandToExecute.CommandTimeout = this._commandTimeout;
                try
                {
                    pCommandToExecute.Transaction = this.Transaction;
                    NpgsqlConnection getConnection = this.GetConnection;
                    lock (getConnection)
                    {
                        pCommandToExecute.Connection = getConnection;
                        sqlDataAdapter = new NpgsqlDataAdapter(pCommandToExecute);
                        sqlDataAdapter.Fill(dataSet, pTableToCreate);
                    }
                    sqlConnection = (NpgsqlConnection)null;
                }
                finally
                {
                    if (sqlDataAdapter != null)
                        sqlDataAdapter.Dispose();
                }
                return dataSet;
            }
            finally
            {
                if (dataSet != null)
                    dataSet.Dispose();
                this.CloseConnection();
            }
        }

        public DataSet RecordSet(NpgsqlCommand pCommandToExecute, string pTableToCreate)
        {
            try
            {
                return this.RecordSet(pCommandToExecute, pTableToCreate, new DataSet());
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
        }

        public DataSet RecordSet(NpgsqlCommand pCommandToExecute)
        {
            try
            {
                return this.RecordSet(pCommandToExecute, "RETORNO");
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
        }


        public DataSet RecordSet(string pQueryToExecute, string pTableToCreate, DataSet pDataSetToFill)
        {
            NpgsqlCommand pCommandToExecute = (NpgsqlCommand)null;
            try
            {
                pCommandToExecute = this.CreateCommand(pQueryToExecute, new Hashtable());
                return this.RecordSet(pCommandToExecute, pTableToCreate, pDataSetToFill);
            }
            finally
            {
                if (pCommandToExecute != null)
                    pCommandToExecute.Dispose();
            }
        }

        public DataSet RecordSet(string pQueryToExecute, string pTableToCreate)
        {
            NpgsqlCommand pCommandToExecute = (NpgsqlCommand)null;
            try
            {
                pCommandToExecute = this.CreateCommand(pQueryToExecute, new Hashtable());
                return this.RecordSet(pCommandToExecute, pTableToCreate);
            }
            finally
            {
                if (pCommandToExecute != null)
                    pCommandToExecute.Dispose();
            }
        }

        public DataSet RecordSet(string pQueryToExecute)
        {
            try
            {
                return this.RecordSet(pQueryToExecute, "RETORNO");
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
        }


        public DataSet RecordSet(string[] pQueryToExecute)
        {
            DataSet dataSet = (DataSet)null;
            try
            {
                dataSet = new DataSet();
                for (int index = 0; index < pQueryToExecute.Length; ++index)
                    dataSet.Merge(this.RecordSet(pQueryToExecute[index], "RETORNO" + (object)index));
                return dataSet;
            }
            finally
            {
                if (dataSet != null)
                    dataSet.Dispose();
            }
        }

        public DataSet RecordSet(NpgsqlCommand[] pQueryToExecute)
        {
            DataSet dataSet = (DataSet)null;
            try
            {
                dataSet = new DataSet();
                for (int index = 0; index < pQueryToExecute.Length; ++index)
                    dataSet.Merge(this.RecordSet(pQueryToExecute[index], "RETORNO" + (object)index));
                return dataSet;
            }
            finally
            {
                if (dataSet != null)
                    dataSet.Dispose();
            }
        }


        public DataSet RecordSet(Hashtable pQueryToExecute)
        {
            DataSet dataSet = (DataSet)null;
            IDictionaryEnumerator dictionaryEnumerator = (IDictionaryEnumerator)null;
            try
            {
                dataSet = new DataSet();
                dictionaryEnumerator = pQueryToExecute.GetEnumerator();
                while (dictionaryEnumerator.MoveNext())
                    dataSet.Merge(this.RecordSet(dictionaryEnumerator.Value.ToString(), dictionaryEnumerator.Key.ToString()));
                return dataSet;
            }
            finally
            {
                if (dataSet != null)
                    dataSet.Dispose();
                if (dictionaryEnumerator != null)
                    ;
            }
        }

        public DataSet RecordSet(string pQueryToExecute, Hashtable pParameters)
        {
            try
            {
                return this.RecordSet(pQueryToExecute, pParameters, "RETORNO");
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
        }

        public DataSet RecordSet(string pQueryToExecute, Hashtable pParameters, string pTableToCreate)
        {
            NpgsqlCommand pCommandToExecute = (NpgsqlCommand)null;
            try
            {
                pCommandToExecute = this.CreateCommand(pQueryToExecute, pParameters);
                return this.RecordSet(pCommandToExecute, pTableToCreate);
            }
            finally
            {
                if (pCommandToExecute != null)
                    pCommandToExecute.Dispose();
            }
        }

        private DataSet RecordSet(string pQueryToExecute, object[] pParameters)
        {
            try
            {
                return this.RecordSet(pQueryToExecute, pParameters, "RETORNO");
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
        }


        private DataSet RecordSet(string pQueryToExecute, object[] pParameters, string pTableToCreate, DataSet pDataSetToFill)
        {
            NpgsqlCommand pCommandToExecute = (NpgsqlCommand)null;
            try
            {
                pCommandToExecute = this.CreateCommand(pQueryToExecute, pParameters);
                return this.RecordSet(pCommandToExecute, pTableToCreate, pDataSetToFill);
            }
            finally
            {
                if (pCommandToExecute != null)
                    pCommandToExecute.Dispose();
            }
        }

        private DataSet RecordSet(string pQueryToExecute, object[] pParameters, string pTableToCreate)
        {
            NpgsqlCommand pCommandToExecute = (NpgsqlCommand)null;
            try
            {
                pCommandToExecute = this.CreateCommand(pQueryToExecute, pParameters);
                return this.RecordSet(pCommandToExecute, pTableToCreate);
            }
            finally
            {
                if (pCommandToExecute != null)
                    pCommandToExecute.Dispose();
            }
        }

        public DataSet RecordSetDataReader(string pCommandToExecute, ref DataSet pDataSetToFill)
        {
            NpgsqlDataReader sqlDataReader = (NpgsqlDataReader)null;
            try
            {
                NpgsqlCommand sqlCommand = new NpgsqlCommand(pCommandToExecute, this.GetConnection);
                sqlCommand.Transaction = this.Transaction;
                if (this._commandTimeout >= 0)
                    sqlCommand.CommandTimeout = this._commandTimeout;
                sqlDataReader = sqlCommand.ExecuteReader();
                if (pDataSetToFill == null)
                    pDataSetToFill = new DataSet();
                if (pDataSetToFill.Tables.Count == 0)
                    pDataSetToFill.Tables.Add();
                while (sqlDataReader.Read())
                {
                    object[] objArray = new object[sqlDataReader.FieldCount];
                    for (int ordinal = 0; ordinal < sqlDataReader.FieldCount; ++ordinal)
                        objArray[ordinal] = sqlDataReader.GetValue(ordinal);
                    pDataSetToFill.Tables[0].Rows.Add(objArray);
                }
                return (DataSet)null;
            }
            finally
            {
                if (sqlDataReader != null)
                    ;
                this.CloseConnection();
            }
        }

        public string[] QueryInsert(DataRow[] pDataRowTableToInsert, string pTableToInsert)
        {
            string str1 = "";
            try
            {
                foreach (DataColumn column in (InternalDataCollectionBase)pDataRowTableToInsert[0].Table.Columns)
                    str1 = str1 + (object)',' + column.ColumnName;
                string str2 = str1.Substring(1);
                return this.QueryInsert(pDataRowTableToInsert, str2.Split(",".ToCharArray()), pTableToInsert);
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
        }

        public string[] QueryInsert(DataTable pDataTableToInsert, string pTableToInsert)
        {
            string str1 = string.Empty;
            try
            {
                foreach (DataColumn column in (InternalDataCollectionBase)pDataTableToInsert.Columns)
                    str1 = str1 + (object)',' + column.ColumnName;
                string str2 = str1.Substring(1);
                return this.QueryInsert(pDataTableToInsert, str2.Split(",".ToCharArray()), pTableToInsert);
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
        }

        public string[] QueryInsert(DataTable pDataTableToInsert, string[] pCamposInsert, string pTableToInsert)
        {
            try
            {
                return this.QueryInsert(pDataTableToInsert.Select(), pCamposInsert, pTableToInsert);
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
        }


        public string[] QueryInsert(DataRow[] pDataTableToInsert, string[] pCamposInsert, string pTableToInsert)
        {
            string str1 = string.Empty;
            try
            {
                string[] strArray = new string[pDataTableToInsert.Length];
                int index1 = 0;
                foreach (DataRow dataRow in pDataTableToInsert)
                {
                    string str2 = "";
                    foreach (string index2 in pCamposInsert)
                    {
                        str1 = str1 + (object)',' + this.FormatColumn(dataRow[index2]);
                        str2 = str2 + (object)',' + index2;
                    }
                    strArray.SetValue((object)("INSERT INTO " + pTableToInsert + " (" + str2.Substring(1) + ") values (" + str1.Substring(1) + ")"), index1);
                    ++index1;
                    str1 = "";
                }
                return strArray;
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
        }

        public string[] QueryUpdate(DataTable pDataTableToInsert, string pTableToInsert, string[] pCamposChave)
        {
            string str1 = string.Empty;
            try
            {
                foreach (DataColumn column in (InternalDataCollectionBase)pDataTableToInsert.Columns)
                    str1 = str1 + (object)',' + column.ColumnName;
                string str2 = str1.Substring(1);
                return this.QueryUpdate(pDataTableToInsert, pTableToInsert, pCamposChave, str2.Split(",".ToCharArray()));
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
        }


        public string[] QueryUpdate(DataTable pDataTableToInsert, string pTableToInsert, string[] pCamposChave, string[] pCamposInsert)
        {
            try
            {
                string[] strArray = new string[pDataTableToInsert.Rows.Count];
                int index1 = 0;
                foreach (DataRow row in (InternalDataCollectionBase)pDataTableToInsert.Rows)
                {
                    string str = "";
                    foreach (string index2 in pCamposInsert)
                        str = str + "," + index2 + " = " + this.FormatColumn(row[index2]);
                    strArray.SetValue((object)("UPDATE  " + pTableToInsert + " SET " + str.Substring(1) + " WHERE " + this.GeraCondicaoWhere(row, pCamposChave)), index1);
                    ++index1;
                }
                return strArray;
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
        }

        public string[] QueryUpdate(DataRow[] pDataTableToInsert, string pTableToInsert, string[] pCamposChave, string[] pCamposInsert)
        {
            try
            {
                string[] strArray = new string[pDataTableToInsert.Length];
                int index1 = 0;
                foreach (DataRow pRow in pDataTableToInsert)
                {
                    string str = "";
                    foreach (string index2 in pCamposInsert)
                        str = str + "," + index2 + " = " + this.FormatColumn(pRow[index2]);
                    strArray.SetValue((object)("UPDATE  " + pTableToInsert + " SET " + str.Substring(1) + " WHERE " + this.GeraCondicaoWhere(pRow, pCamposChave)), index1);
                    ++index1;
                }
                return strArray;
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
        }

        private NpgsqlCommand CreateCommand(string pQueryToExecute, object[] pParameters)
        {
            NpgsqlCommand sqlCommand = (NpgsqlCommand)null;
            try
            {
                int num = 0;
                sqlCommand = new NpgsqlCommand(pQueryToExecute);
                foreach (object pParameter in pParameters)
                {
                    sqlCommand.Parameters.AddWithValue(num.ToString(), this.GetNullValue(pParameter));
                    ++num;
                }
                return sqlCommand;
            }
            finally
            {
                if (sqlCommand != null)
                    ;
            }
        }


        private NpgsqlCommand CreateCommand(string pQueryToExecute, Hashtable pParameters)
        {
            NpgsqlCommand sqlCommand = (NpgsqlCommand)null;
            NpgsqlParameter sqlParameter = (NpgsqlParameter)null;
            try
            {
                IDictionaryEnumerator enumerator = pParameters.GetEnumerator();
                sqlCommand = new NpgsqlCommand(pQueryToExecute);
                while (enumerator.MoveNext())
                {
                    sqlParameter = new NpgsqlParameter(enumerator.Key.ToString(), this.GetNullValue(enumerator.Value));
                    sqlCommand.Parameters.Add(sqlParameter);
                }
                return sqlCommand;
            }
            finally
            {
                if (sqlCommand != null)
                    ;
                if (sqlParameter != null)
                    ;
            }
        }


        public string GeraCondicaoWhere(DataRow pRow, string[] pCamposChave)
        {
            string str1 = string.Empty;
            try
            {
                foreach (string index in pCamposChave)
                {
                    string str2 = this.FormatColumn(pRow[index]);
                    string str3 = !str2.ToUpper().ToString().Equals("NULL") ? " = " + str2 : "IS NULL";
                    str1 = str1 + " AND " + index + str3;
                }
                return str1.Substring(4);
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
        }

        public string FormatColumn(object Valor)
        {
            try
            {
                string str1 = "System.UInt16,System.UInt32,System.UInt64,System.Decimal,System.Byte,System.Int16,System.Int32,System.Int64,System.SByte,System.Single,System.Double";
                string str2 = "System.Char,System.String";
                string str3 = "System.DateTime,System.TimeSpan";
                return str1.IndexOf(Valor.GetType().ToString()) <= 0 ? (str2.IndexOf(Valor.GetType().ToString()) <= 0 ? (str3.IndexOf(Valor.GetType().ToString()) <= 0 ? "Null" : Valor.ToString()) : "'" + Valor.ToString().Replace("'", "''") + "'") : Valor.ToString();
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
        }

        public object GetNullValue(object Valor)
        {
            try
            {
                object obj1 = (object)DBNull.Value;
                object obj2;
                switch (Valor.GetType().ToString())
                {
                    case "System.UInt16":
                        obj2 = (int)Convert.ToUInt16(Valor) == 0 ? (object)null : Valor;
                        break;
                    case "System.UInt32":
                        obj2 = (int)Convert.ToUInt32(Valor) == 0 ? (object)null : Valor;
                        break;
                    case "System.UInt64":
                        obj2 = (long)Convert.ToUInt64(Valor) == 0L ? (object)null : Valor;
                        break;
                    case "System.Decimal":
                        obj2 = Convert.ToDecimal(Valor) == new Decimal(-1, -1, -1, true, (byte)0) ? (object)null : Valor;
                        break;
                    case "System.Byte":
                        obj2 = (int)Convert.ToByte(Valor) == 0 ? (object)null : Valor;
                        break;
                    case "System.Int16":
                        obj2 = (int)Convert.ToInt16(Valor) == (int)short.MinValue ? (object)null : Valor;
                        break;
                    case "System.Int32":
                        obj2 = Convert.ToInt32(Valor) == int.MinValue ? (object)null : Valor;
                        break;
                    case "System.Int64":
                        obj2 = Convert.ToInt64(Valor) == long.MinValue ? (object)null : Valor;
                        break;
                    case "System.SByte":
                        obj2 = (int)Convert.ToSByte(Valor) == (int)sbyte.MinValue ? (object)null : Valor;
                        break;
                    case "System.Single":
                        obj2 = (double)Convert.ToSingle(Valor) == -3.40282346638529E+38 ? (object)null : Valor;
                        break;
                    case "System.Double":
                        obj2 = Convert.ToDouble(Valor) == double.MinValue ? (object)null : Valor;
                        break;
                    default:
                        obj2 = Valor;
                        break;
                }
                if (obj2 == null)
                    obj2 = (object)DBNull.Value;
                return obj2;
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
        }

        private void Dispose(bool pDisposing)
        {
            if (_disposed)
                return;
            this.CloseConnection();
            if (pDisposing)
            {
                if (this._transaction != null)
                    this._transaction = (NpgsqlTransaction)null;

            }
            this._disposed = true;

        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);

        }
    }
}
