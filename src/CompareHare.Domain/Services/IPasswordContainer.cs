namespace CompareHare.Domain.Services
{
    public interface IPasswordContainer
    {
        string PasswordSalt { get; set; }
        string PasswordHash { get; set; }
    }
}
