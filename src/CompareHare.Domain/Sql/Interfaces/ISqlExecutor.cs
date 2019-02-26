#region usings

using System.Data.Common;
using System.Threading.Tasks;

#endregion

namespace CompareHare.Domain.Sql.Interfaces
{
    public interface ISqlExecutor
    {
        Task Execute(ISqlCommand sqlCommand, bool startTransaction = true);
        Task<TReturn> Execute<TReturn>(ISqlCommand<TReturn> sqlCommand, bool startTransaction = true);
        Task<TReturn> Execute<TReturn>(IBufferedSqlQuery<TReturn> sqlQuery);
        Task<TReturn> Execute<TReturn>(IUnbufferedSqlQuery<TReturn> sqlQuery, DbConnection connection);
    }
}
