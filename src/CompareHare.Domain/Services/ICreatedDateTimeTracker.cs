namespace CompareHare.Domain.Services
{
    public interface ICreatedDateTimeTracker
    {
        DateTimeOffset CreatedDate { set; }
    }
}