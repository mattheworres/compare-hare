#region usings

using System.Threading.Tasks;

#endregion

namespace CompareHare.Domain.Sql.Interfaces
{
    public interface ISqlExecutor
    {
        Task Execute(ISqlCommand sqlCommand, SqlExecutionContext context = null);
        void Execute(ISyncSqlCommand sqlCommand, SqlExecutionContext context = null);
        Task<TReturn> Execute<TReturn>(ISqlCommand<TReturn> sqlCommand, SqlExecutionContext context = null);
        Task<TReturn> Execute<TReturn>(ISqlQuery<TReturn> sqlQuery, SqlExecutionContext context = null);
    }
}
