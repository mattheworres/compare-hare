using System.Data.Common;

namespace CompareHare.Domain.Sql.Interfaces
{
    public interface ISqlCommand
    {
        Task Execute(DbConnection connection, DbTransaction transaction);
    }

    public interface ISqlCommand<TReturn>
    {
        Task<TReturn> Execute(DbConnection connection, DbTransaction transaction);
    }
}
