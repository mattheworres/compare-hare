namespace CompareHare.Api.Features.Authentication.Models
{
    public class LogInModel
    {
        public LogInModel()
        {
            Email = "";
            Password = "";
        }

        public string Email { get; set; }
        public string Password { get; set; }
    }
}
