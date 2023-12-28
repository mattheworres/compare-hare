namespace CompareHare.Domain.Services
{
    public interface IModifiedDateTimeTracker
    {
        DateTimeOffset? ModifiedDate { set; }
    }
}