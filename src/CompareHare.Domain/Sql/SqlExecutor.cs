#region usings

using System;
using System.Data.Common;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using CompareHare.Domain.Sql.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Dapper;
using Microsoft.EntityFrameworkCore;

#endregion

namespace CompareHare.Domain.Sql
{
    public class SqlExecutor : ISqlExecutor
    {
        private readonly Lazy<Func<Owned<DbConnection>>> _connectionFactory;
        private readonly Lazy<IExecutionStrategy> _executionStrategy;

        public SqlExecutor(
            Lazy<IExecutionStrategy> executionStrategy,
            Lazy<Func<Owned<DbConnection>>> connectionFactory)
        {
            _executionStrategy = executionStrategy;
            _connectionFactory = connectionFactory;
        }

        public async Task Execute(ISqlCommand sqlCommand, SqlExecutionContext context = null)
        {
            if (context != null)
            {
                await sqlCommand.Execute(context.Connection, context.Transaction);
                return;
            }

            await _executionStrategy.Value.ExecuteAsync(
                async () =>
                {
                    using (var connection = _connectionFactory.Value.Invoke())
                    {
                        await connection.Value.OpenAsync();

                        var dbTransaction = connection.Value.BeginTransaction();

                        try
                        {
                            await sqlCommand.Execute(connection.Value, dbTransaction);

                            dbTransaction.Commit();
                        }
                        finally
                        {
                            dbTransaction.Dispose();
                        }
                    }
                });
        }

        public void Execute(ISyncSqlCommand sqlCommand, SqlExecutionContext context = null)
        {
            if (context != null)
            {
                sqlCommand.Execute(context.Connection, context.Transaction);
                return;
            }

            _executionStrategy.Value.Execute(
                () =>
                {
                    using (var connection = _connectionFactory.Value.Invoke())
                    {
                        connection.Value.Open();

                        var dbTransaction = connection.Value.BeginTransaction();

                        try
                        {
                            sqlCommand.Execute(connection.Value, dbTransaction);

                            dbTransaction.Commit();
                        }
                        finally
                        {
                            dbTransaction.Dispose();
                        }
                    }
                });
        }

        public async Task<TReturn> Execute<TReturn>(ISqlCommand<TReturn> sqlCommand, SqlExecutionContext context = null)
        {
            if (context != null) return await sqlCommand.Execute(context.Connection, context.Transaction);

            return await _executionStrategy.Value.ExecuteAsync(
                       async () =>
                       {
                           using (var connection = _connectionFactory.Value.Invoke())
                           {
                               await connection.Value.OpenAsync();

                               var dbTransaction = connection.Value.BeginTransaction();

                               try
                               {
                                   var result = await sqlCommand.Execute(connection.Value, dbTransaction);

                                   dbTransaction.Commit();

                                   return result;
                               }
                               finally
                               {
                                   dbTransaction.Dispose();
                               }
                           }
                       });
        }

        public async Task<TReturn> Execute<TReturn>(ISqlQuery<TReturn> sqlQuery, SqlExecutionContext context = null)
        {
            if (context != null) return await sqlQuery.Execute(context.Connection, context.Transaction);

            return await _executionStrategy.Value.ExecuteAsync(
                       async () =>
                       {
                           using (var connection = _connectionFactory.Value.Invoke())
                           {
                               await connection.Value.OpenAsync();

                               var dbTransaction = connection.Value.BeginTransaction();

                               try
                               {
                                   var result = await sqlQuery.Execute(connection.Value, dbTransaction);

                                   dbTransaction.Commit();

                                   return result;
                               }
                               finally
                               {
                                   dbTransaction.Dispose();
                               }
                           }
                       });
        }
    }
}
