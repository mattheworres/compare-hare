#region usings

using System.Data.Common;
using System.Threading.Tasks;

#endregion

namespace CompareHare.Domain.Sql.Interfaces
{
    public interface ISqlCommand
    {
        Task Execute(DbConnection connection);
    }

    public interface ISqlCommand<TReturn>
    {
        Task<TReturn> Execute(DbConnection connection);
    }
}
