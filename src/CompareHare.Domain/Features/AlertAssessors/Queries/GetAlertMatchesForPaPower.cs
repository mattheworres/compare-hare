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
            var selector = builder.AddTemplate("SELECT UtiltyPriceHistoryId FROM UtilityPrices /**where**/");

            builder.Where("StateUtilityIndexId = @Id", new {Id = _stateUtilityIndexId});

            if (_alert.MinimumPrice.HasValue) {
                builder.Where("PricePerUnit > @Value", new {Value = _alert.MinimumPrice.Value});
            }

            if (_alert.MaximumPrice.HasValue) {
                builder.Where("PricePerUnit < @Value", new { Value = _alert.MaximumPrice.Value });
            }

            if (_alert.HasRenewable.HasValue) {
                builder.Where("HasRenewable = @Flag", new {Flag = _alert.HasRenewable.Value});

                if (_alert.HasRenewable.Value) {
                    if (_alert.MinimumRenewablePercent.HasValue) {
                        builder.Where("RenewablePercentage > @Value", new {Value = _alert.MinimumRenewablePercent});
                    }

                    if (_alert.MinimumRenewablePercent.HasValue) {
                        builder.Where("RenewablePercentage < @Value", new {Value = _alert.MaximumRenewablePercent});
                    }
                }
            }

            if (_alert.HasCancellationFee.HasValue) {
                builder.Where("HasCancellationFee = @Flag", new {Flag = _alert.HasCancellationFee.Value});
            }

            if (_alert.HasMonthlyFee.HasValue) {
                builder.Where("HasMonthlyFee = @Flag", new {Flag = _alert.HasMonthlyFee.Value});
            }

            if (_alert.HasEnrollmentFee.HasValue) {
                builder.Where("HasEnrollmentFee = @Flag", new {Flag = _alert.HasEnrollmentFee.Value});
            }

            if (_alert.HasNetMetering.HasValue) {
                builder.Where("HasNetMetering = @Flag", new {Flag = _alert.HasNetMetering.Value});
            }

            if (_alert.RequiresDeposit.HasValue) {
                builder.Where("RequiresDeposit = @Flag", new {Flag = _alert.RequiresDeposit.Value});
            }

            if (_alert.HasBulkDiscounts.HasValue) {
                builder.Where("HasBulkDiscounts = @Flag", new {Flag = _alert.HasBulkDiscounts.Value});
            }

            return await connection.QueryAsync<int>(selector.RawSql, transaction: transaction);
        }
    }
}
