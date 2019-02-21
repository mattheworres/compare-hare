#region usings

using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Transactions;
using Autofac.Features.OwnedInstances;
using CompareHare.Domain.Sql.Interfaces;

#endregion

namespace CompareHare.Domain.Sql
{
    public class SqlExecutor : ISqlExecutor
    {
        private readonly Lazy<Func<Owned<DbConnection>>> _connectionFactory;

        public SqlExecutor(
            Lazy<Func<Owned<DbConnection>>> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task Execute(ISqlCommand sqlCommand, bool startTransaction)
        {
            if (!startTransaction)
            {
                await Execute(sqlCommand, null);
                return;
            }

            await Execute(sqlCommand, null);
        }

        private async Task Execute(ISqlCommand sqlCommand, TransactionScope transactionScope)
        {
            using (var connection = _connectionFactory.Value.Invoke())
            {
                await connection.Value.OpenAsync();
                await sqlCommand.Execute(connection.Value);
                transactionScope?.Complete();
                connection.Value.Close();
            }
        }

        public async Task<TReturn> Execute<TReturn>(ISqlCommand<TReturn> sqlCommand, bool startTransaction)
        {
            if (!startTransaction) return await Execute(sqlCommand, null);

            var result = await Execute(sqlCommand);
            return result;
        }

        private async Task<TReturn> Execute<TReturn>(ISqlCommand<TReturn> sqlCommand)
        {
            using (var connection = _connectionFactory.Value.Invoke())
            {
                await connection.Value.OpenAsync();
                var result = await sqlCommand.Execute(connection.Value);
                connection.Value.Close();
                return result;
            }
        }

        private async Task<TReturn> Execute<TReturn>(ISqlCommand<TReturn> sqlCommand, TransactionScope transactionScope)
        {
            using (var connection = _connectionFactory.Value.Invoke())
            {
                await connection.Value.OpenAsync();
                var result = await sqlCommand.Execute(connection.Value);
                transactionScope?.Complete();
                connection.Value.Close();
                return result;
            }
        }

        public async Task<TReturn> Execute<TReturn>(IBufferedSqlQuery<TReturn> sqlQuery)
        {
            using (var connection = _connectionFactory.Value.Invoke())
            {
                await connection.Value.OpenAsync();
                var result = await sqlQuery.Execute(connection.Value);
                connection.Value.Close();
                return result;
            }
        }

        public async Task<TReturn> Execute<TReturn>(IUnbufferedSqlQuery<TReturn> sqlQuery, DbConnection connection)
        {
            if (connection.State == ConnectionState.Closed)  await connection.OpenAsync();
            return await sqlQuery.Execute(connection);
        }
    }
}
