using System.Data.Common;

namespace CompareHare.Domain.Sql.Interfaces
{
    public interface ISqlQuery<TReturn>
    {
        Task<TReturn> Execute(DbConnection connection, DbTransaction transaction);
    }
}
