

namespace CompareHare.Domain.Sql.Interfaces
{
    public interface ISqlExecutor
    {
        Task Execute(ISqlCommand sqlCommand, SqlExecutionContext? context);
        void Execute(ISyncSqlCommand sqlCommand, SqlExecutionContext? context);
        Task<TReturn> Execute<TReturn>(ISqlCommand<TReturn> sqlCommand, SqlExecutionContext? context);
        Task<TReturn> Execute<TReturn>(ISqlQuery<TReturn> sqlQuery, SqlExecutionContext? context);
    }
}
