#region usings

using System.Data.Common;
using System.Threading.Tasks;

#endregion

namespace CompareHare.Domain.Sql.Interfaces
{
    public interface ISqlQuery<TReturn>
    {
        Task<TReturn> Execute(DbConnection connection);
    }
}
