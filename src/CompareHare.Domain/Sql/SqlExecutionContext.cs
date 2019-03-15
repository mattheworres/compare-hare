using System.Data.Common;

namespace CompareHare.Domain.Sql
{
    public class SqlExecutionContext
    {
        public SqlExecutionContext(
            DbConnection connection,
            DbTransaction transaction = null)
        {
            Connection = connection;
            Transaction = transaction;
        }

        public DbConnection Connection { get; }
        public DbTransaction Transaction { get; }
    }
}
