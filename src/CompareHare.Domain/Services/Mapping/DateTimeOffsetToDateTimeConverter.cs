using AutoMapper;

namespace CompareHare.Domain.Services.Mapping
{
    public class DateTimeOffsetToDateTimeConverter : IValueConverter<DateTimeOffset, DateTime>
    {
        public DateTime Convert(DateTimeOffset sourceMember, ResolutionContext context)
        {
            string dateTimeString = sourceMember.ToString();
            return DateTimeOffset.Parse(dateTimeString).UtcDateTime;
        }
    }
}
