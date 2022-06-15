using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Victory.DataAccess
{
    public interface IDataContext : IDisposable
    {
        string ConnectionString { get; }

        #region Sync Methods
        List<IDictionary<string, object>> ExecuteSql(string sql);
        List<T> ExecuteSql<T>(string sql);
        IDataReader ExecuteReader(string sql);
        List<T> ExecuteSql<T>(string sql, IMapper mapper);
        int ExecuteProcedure(string storedProcedureName, object parameters = null);
        List<IDictionary<string, object>> ExecuteReaderProcedure(string storedProcedureName, object parameters = null);
        List<T> ExecuteReaderProcedure<T>(string storedProcedureName, object parameters = null);
        List<T> ExecuteReaderProcedure<T>(string storedProcedureName, IMapper mapper, object parameters = null);
        (List<T>, List<U>) ExecuteReaderProcedure<T, U>(string storedProcedureName, object parameters = null);
        #endregion

        //#region Async Methods
        //Task<List<IDictionary<string, object>>> ExecuteSqlAsync(string sql);
        //Task<List<T>> ExecuteSqlAsync<T>(string sql);
        //Task<List<T>> ExecuteSqlAsync<T>(string sql, IMapper mapper);
        //Task<int> ExecuteProcedureAsync(string storedProcedureName, object parameters = null);
        //Task<List<IDictionary<string, object>>> ExecuteReaderProcedureAsync(string storedProcedureName, object parameters = null);
        //Task<List<T>> ExecuteReaderProcedureAsync<T>(string storedProcedureName, object parameters = null);
        //Task<List<T>> ExecuteReaderProcedureAsync<T>(string storedProcedureName, IMapper mapper, object parameters = null);
        //#endregion

        IDbConnection Connection { get; }
        void OpenConnection();
        IDbTransaction BeginTransaction();
        void Commit();
        void Rollback();
    }
}
