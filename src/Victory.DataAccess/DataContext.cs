using AutoMapper;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Victory.DataAccess
{
    public sealed class DataContext<T> : IDataContext where T: DbConnection
    {
        #region Fields

        private readonly string _connectionString;
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;

        #endregion

        #region Ctor

        public DataContext(string connectionString)
        {
            _connectionString = connectionString;
            _connection = (T)Activator.CreateInstance(typeof(T), connectionString);
        }

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }
        #endregion

        #region Sync Methods

        public List<IDictionary<string, object>> ExecuteSql(string sql)
        {
            var res = _connection.Query(sql,
                transaction: _transaction,
                commandType: CommandType.Text,
                commandTimeout: _connection.ConnectionTimeout);

            if (res == null)
                return null;

            if (res.Count() == 0)
                return Enumerable.Empty<IDictionary<string, object>>().ToList();

            return ((IEnumerable<IDictionary<string, object>>)res).ToList();
        }

        public List<T> ExecuteSql<T>(string sql)
        {
            var res = _connection.Query<T>(sql,
                transaction: _transaction,
                commandType: CommandType.Text,
                commandTimeout: _connection.ConnectionTimeout).ToList();

            return res;
        }

        public List<T> ExecuteSql<T>(string sql, IMapper mapper)
        {
            var res = (IEnumerable<IDictionary<string, object>>)_connection.Query(sql,
               transaction: _transaction,
               commandType: CommandType.Text,
               commandTimeout: _connection.ConnectionTimeout);

            return mapper.Map<List<T>>(res);
        }

        /// <summary>
        /// Execute with stored procedure by name
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        public int ExecuteProcedure(string storedProcedureName, object parameters = null)
        {
            var res = _connection.Execute(sql: storedProcedureName,
                param: parameters,
                transaction: _transaction,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _connection.ConnectionTimeout);

            return res;
        }


        public List<IDictionary<string, object>> ExecuteReaderProcedure(string storedProcedureName, object parameters = null)
        {
            var res = (IEnumerable<IDictionary<string, object>>)_connection.Query(storedProcedureName,
                param: parameters,
                transaction: _transaction,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _connection.ConnectionTimeout);

            return res?.ToList();
        }

        public List<T> ExecuteReaderProcedure<T>(string storedProcedureName, IMapper mapper, object parameters = null)
        {
            var res = (IEnumerable<IDictionary<string, object>>)_connection.Query(storedProcedureName,
               param: parameters,
               transaction: _transaction,
               commandType: CommandType.StoredProcedure,
               commandTimeout: _connection.ConnectionTimeout);

            return mapper.Map<List<T>>(res);
        }

        public List<T> ExecuteReaderProcedure<T>(string storedProcedureName, object parameters = null)
        {
            var res = _connection.Query<T>(storedProcedureName,
                param: parameters,
                transaction: _transaction,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _connection.ConnectionTimeout).ToList();

            return res;
        }

        public (List<T>, List<U>) ExecuteReaderProcedure<T, U>(string storedProcedureName, object parameters = null)
        {
            var dataSets = _connection.QueryMultiple(storedProcedureName,
                    param: parameters,
                    transaction: _transaction,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: _connection.ConnectionTimeout);

            var firstDataSet = dataSets.Read<T>().ToList();
            var secondDataSet = dataSets.Read<U>().ToList();

            return (firstDataSet, secondDataSet);
        }

        #endregion

        #region Async Methods


        public async Task<List<IDictionary<string, object>>> ExecuteSqlAsync(string sql)
        {
            var res = await _connection.QueryAsync(sql,
                transaction: _transaction,
                commandType: CommandType.Text,
                commandTimeout: _connection.ConnectionTimeout);

            //has to be done like this because Dapper from some reason when QueryAsync is called does not
            //retrieve the object in the same way as Query method. So the cast has to be done element by element
            var resConverted = res?.Select(x => (IDictionary<string, object>)x).ToList();

            return resConverted;
        }

        public async Task<List<T>> ExecuteSqlAsync<T>(string sql)
        {
            var res = await _connection.QueryAsync<T>(sql,
                transaction: _transaction,
                commandType: CommandType.Text,
                commandTimeout: _connection.ConnectionTimeout);

            return res?.ToList();
        }

        public async Task<List<T>> ExecuteSqlAsync<T>(string sql, IMapper mapper)
        {
            var res = await _connection.QueryAsync(sql,
               transaction: _transaction,
               commandType: CommandType.Text,
               commandTimeout: _connection.ConnectionTimeout);

            return mapper.Map<List<T>>(res);
        }

        public async Task<List<IDictionary<string, object>>> ExecuteReaderProcedureAsync(string storedProcedureName, object parameters = null)
        {
            var res = await _connection.QueryAsync(storedProcedureName,
                param: parameters,
                transaction: _transaction,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _connection.ConnectionTimeout);

            //has to be done like this because Dapper from some reason when QueryAsync is called does not
            //retrieve the object in the same way as Query method. So the cast has to be done element by element
            var resConverted = res?.Select(x => (IDictionary<string, object>)x).ToList();

            return resConverted;
        }

        public async Task<List<T>> ExecuteReaderProcedureAsync<T>(string storedProcedureName, object parameters = null)
        {
            var res = await _connection.QueryAsync<T>(storedProcedureName,
                    param: parameters,
                    transaction: _transaction,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: _connection.ConnectionTimeout);

            return res?.ToList();
        }

        public async Task<List<T>> ExecuteReaderProcedureAsync<T>(string storedProcedureName, IMapper mapper, object parameters = null)
        {
            
            var res = await _connection.QueryAsync(storedProcedureName,
                param: parameters,
                transaction: _transaction,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _connection.ConnectionTimeout);

            return mapper.Map<List<T>>(res);           
        }

        public async Task<int> ExecuteProcedureAsync(string storedProcedureName, object parameters = null)
        {
            var res = await _connection.ExecuteAsync(sql: storedProcedureName,
                param: parameters,
                transaction: _transaction,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _connection.ConnectionTimeout);

            return res;
        }

        #endregion

        #region Context Management
        /// <summary>
        /// Begin transcation scope
        /// </summary>
        /// <returns></returns>
        public IDbTransaction BeginTransaction()
        {
            OpenConnection();
            if (_transaction != null) throw new Exception("Transaction already opened! Please COMMIT/ROLLBACK transaction before opening a new one!");
            _transaction = _connection.BeginTransaction();
            return _transaction;
        }

        public void Commit()
        {
            if (_transaction != null)
            {
                try
                {
                    _transaction.Commit();
                }
                catch
                {
                    _transaction.Rollback();
                    throw;
                }
                finally
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        public void Rollback()
        {
            if (_transaction != null)
            {
                try
                {
                    _transaction.Rollback();
                }
                finally
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        /// <summary>
        /// Open connection
        /// </summary>
        public void OpenConnection()
        {
            if (_connection != null &&
                _connection.State != ConnectionState.Open &&
                _connection.State != ConnectionState.Connecting)
                _connection.Open();
        }

        /// <summary>
        /// Close connection 
        /// </summary>
        public void CloseConnection()
        {
            if (_connection != null &&
                _connection.State != ConnectionState.Closed)
                _connection.Close();
        }

        /// <summary>
        /// Gets the current connection
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                return _connection;
            }
        }


        /// <summary>
        /// Dispose the current connection
        /// </summary>
        public void Dispose()
        {
            _connection?.Dispose();
        }

        #endregion
    }
}
