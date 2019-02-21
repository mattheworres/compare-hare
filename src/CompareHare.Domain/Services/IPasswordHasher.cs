namespace CompareHare.Domain.Services
{
    public interface IPasswordHasher
    {
        void SetPassword(string password, IPasswordContainer passwordContainer);
        string HashPassword(string password, IPasswordContainer passwordContainer);
    }
}
