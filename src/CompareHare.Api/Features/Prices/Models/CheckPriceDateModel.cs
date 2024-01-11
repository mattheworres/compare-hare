using System;

namespace CompareHare.Api.Features.Prices.Models
{
    public class CheckPriceDateModel
    {
        public CheckPriceDateModel(int trackedProductRetailerId, DateTimeOffset date)
        {
            Date = date;
            TrackedProductRetailerId = trackedProductRetailerId;
        }

        public int TrackedProductRetailerId { get; }
        public DateTimeOffset Date { get; }
    }
}