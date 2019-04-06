using System;
using System.Collections.Generic;
using AutoMapper;
using CompareHare.Api.Features.Alerts.Models;
using CompareHare.Domain.Entities;

namespace CompareHare.Api.Features.Shared.Mapping
{
    public class AlertFeatureResolver : IValueResolver<Alert, object, IEnumerable<AlertDisplayParameter>>
    {
        public IEnumerable<AlertDisplayParameter> Resolve(Alert source, object destination, IEnumerable<AlertDisplayParameter> destMember, ResolutionContext context)
        {
            var list = new List<AlertDisplayParameter>();

            if (source.MinimumPrice.HasValue || source.MaximumPrice.HasValue) {
                var hasMin = source.MinimumPrice.HasValue;
                var hasMax = source.MaximumPrice.HasValue;
                var min = TrimNullableDecimal(source.MinimumPrice, 3);
                var max = TrimNullableDecimal(source.MaximumPrice, 3);

                var priceLabel = "Price";

                if (hasMin && !hasMax) {
                    list.Add(new AlertDisplayParameter(priceLabel, $"min ${min}/unit"));
                } else if (hasMin && hasMax) {
                    list.Add(new AlertDisplayParameter(priceLabel, $"${min} to ${max}/unit"));
                } else {
                    list.Add(new AlertDisplayParameter(priceLabel, $"max ${max}/unit"));
                }
            }

            if (source.MinimumMonthLength.HasValue || source.MaximumMonthLength.HasValue) {
                var hasMin = source.MinimumMonthLength.HasValue;
                var hasMax = source.MaximumMonthLength.HasValue;
                var monthLabel = "Contract Length";

                if (hasMin && !hasMax) {
                    list.Add(new AlertDisplayParameter(monthLabel, $"{source.MinimumMonthLength.Value} months min"));
                } else if (hasMin && hasMax) {
                    list.Add(new AlertDisplayParameter(monthLabel, $"{source.MinimumMonthLength.Value} - {source.MaximumMonthLength.Value} months"));
                } else {
                    list.Add(new AlertDisplayParameter(monthLabel, $"{source.MaximumMonthLength.Value} months max"));
                }
            }

            if (source.HasRenewable.HasValue) {
                var renewableLabel = "Has Renewables";
                if (source.HasRenewable.Value) {
                    var hasNonZeroMin = source.MinimumRenewablePercent.HasValue && source.MinimumRenewablePercent.Value > 0;
                    var hasNon100Max = source.MaximumRenewablePercent.HasValue && source.MaximumRenewablePercent.Value < 100;
                    var min = source.MinimumRenewablePercent.HasValue ? source.MinimumRenewablePercent.Value : 0;
                    var max = source.MaximumRenewablePercent.HasValue ? source.MaximumRenewablePercent : 0;

                    list.Add(new AlertDisplayParameter(renewableLabel, $"{min}% - {max}%"));
                } else {
                    list.Add(new AlertDisplayParameter(renewableLabel, "No"));
                }
            }

            if (source.HasCancellationFee.HasValue) {
                list.Add(new AlertDisplayParameter("Cancellation Fee", CoalesceBooleanYesNo(source.HasCancellationFee.Value)));
            }

            if (source.HasMonthlyFee.HasValue) {
                list.Add(new AlertDisplayParameter("Monthly Fee", CoalesceBooleanYesNo(source.HasMonthlyFee.Value)));
            }

            if (source.HasEnrollmentFee.HasValue) {
                list.Add(new AlertDisplayParameter("Enrollment Fee", CoalesceBooleanYesNo(source.HasEnrollmentFee.Value)));
            }

            if (source.RequiresDeposit.HasValue) {
                list.Add(new AlertDisplayParameter("Requires Deposit", CoalesceBooleanYesNo(source.RequiresDeposit.Value)));
            }

            if (source.HasBulkDiscounts.HasValue) {
                list.Add(new AlertDisplayParameter("Bulk Discounts", CoalesceBooleanYesNo(source.HasBulkDiscounts.Value)));
            }

            if (source.HasNetMetering.HasValue) {
                list.Add(new AlertDisplayParameter("Net Metering", CoalesceBooleanYesNo(source.HasNetMetering.Value)));
            }

            return list;
        }

        private string CoalesceBooleanYesNo(bool boolean) {
            return boolean ? "Yes" : "No";
        }

        private string TrimNullableFloat(float? floaty, int digits) {
            var value = floaty.HasValue ? floaty.Value : 0f;
            var mult = Math.Pow(10.0, digits);
            var result = Math.Truncate(mult * value) / mult;
            return ((float)result).ToString();
        }

        private string TrimNullableDecimal(decimal? deci, int digits) {
            var value = deci.HasValue ? deci.Value : 0m;
            var mult = Math.Pow(10.0, digits);
            var result = Math.Truncate(mult * (double)value) / mult;
            return ((decimal)result).ToString();
        }
    }
}
