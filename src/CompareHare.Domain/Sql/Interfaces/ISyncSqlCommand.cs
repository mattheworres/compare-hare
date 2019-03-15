using System.Data.Common;

namespace CompareHare.Domain.Sql.Interfaces
{
    public interface ISyncSqlCommand
    {
        void Execute(DbConnection connection, DbTransaction transaction);
    }
}
