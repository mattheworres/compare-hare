using System.Data.Common;
using CompareHare.Domain.Sql.Interfaces;
using Dapper;

namespace CompareHare.Domain.Features.AlertAssessors.Queries
{
    public class RemoveAllPriceLinkers : ISqlQuery<IEnumerable<int>>
    {
        private readonly int _alertMatchId;

        public RemoveAllPriceLinkers(int alertMatchId)
        {
            _alertMatchId = alertMatchId;
        }

        public async Task<IEnumerable<int>> Execute(DbConnection connection, DbTransaction transaction)
        {
            var builder = new SqlBuilder();
            var selector = builder.AddTemplate("DELETE FROM AlertMatchUtilityPriceHistories /**where**/");

            builder.Where("AlertMatchId = @Id", new {Id = _alertMatchId});

            return await connection.QueryAsync<int>(selector.RawSql, selector.Parameters, transaction: transaction);
        }
    }
}
