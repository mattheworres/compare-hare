using System.Data.Common;
using System.Threading.Tasks;

namespace CompareHare.Domain.Sql.Interfaces
{
    public interface ISqlQuery<TReturn>
    {
        Task<TReturn> Execute(DbConnection connection, DbTransaction transaction);
    }
}
