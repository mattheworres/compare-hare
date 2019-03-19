using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Sql.Interfaces;
using Dapper;

namespace CompareHare.Domain.Features.AlertAssessors.Queries
{
    public class GetAlertMatchesForPaPower : ISqlQuery<IEnumerable<int>>
    {
        private readonly Alert _alert;
        private readonly int _stateUtilityIndexId;

        public GetAlertMatchesForPaPower(Alert alert, int stateUtilityIndexId)
        {
            _alert = alert;
            _stateUtilityIndexId = stateUtilityIndexId;
        }

        public async Task<IEnumerable<int>> Execute(DbConnection connection, DbTransaction transaction)
        {
            var builder = new SqlBuilder();
            var selector = builder.AddTemplate("SELECT UtilityPrices.UtilityPriceHistoryId FROM UtilityPrices /**where**/");

            builder.Where("StateUtilityIndexId = @Id", new {Id = _stateUtilityIndexId});

            if (_alert.MinimumPrice.HasValue) {
                builder.Where("PricePerUnit > @MinValue", new {MinValue = _alert.MinimumPrice.Value});
            }

            if (_alert.MaximumPrice.HasValue) {
                builder.Where("PricePerUnit < @MaxValue", new {MaxValue = _alert.MaximumPrice.Value});
            }

            if (_alert.HasRenewable.HasValue) {
                builder.Where("HasRenewable = @HasRenewable", new {HasRenewable = _alert.HasRenewable.Value});

                if (_alert.HasRenewable.Value) {
                    if (_alert.MinimumRenewablePercent.HasValue) {
                        builder.Where("RenewablePercentage > @MinRenPerc", new {MinRenPerc = _alert.MinimumRenewablePercent});
                    }

                    if (_alert.MinimumRenewablePercent.HasValue) {
                        builder.Where("RenewablePercentage < @MaxRenPerc", new {MaxRenPerc = _alert.MaximumRenewablePercent});
                    }
                }
            }

            if (_alert.HasCancellationFee.HasValue) {
                builder.Where("HasCancellationFee = @HasCancellation", new {HasCancellation = _alert.HasCancellationFee.Value});
            }

            if (_alert.HasMonthlyFee.HasValue) {
                builder.Where("HasMonthlyFee = @HasMonthly", new {HasMonthly = _alert.HasMonthlyFee.Value});
            }

            if (_alert.HasEnrollmentFee.HasValue) {
                builder.Where("HasEnrollmentFee = @HasEnrollment", new {HasEnrollment = _alert.HasEnrollmentFee.Value});
            }

            if (_alert.HasNetMetering.HasValue) {
                builder.Where("HasNetMetering = @HasNetMetering", new {HasNetMetering = _alert.HasNetMetering.Value});
            }

            if (_alert.RequiresDeposit.HasValue) {
                builder.Where("RequiresDeposit = @RequiresDep", new {RequiresDep = _alert.RequiresDeposit.Value});
            }

            if (_alert.HasBulkDiscounts.HasValue) {
                builder.Where("HasBulkDiscounts = @HasBulk", new {HasBulk = _alert.HasBulkDiscounts.Value});
            }

            return await connection.QueryAsync<int>(selector.RawSql, selector.Parameters, transaction: transaction);
        }
    }
}
