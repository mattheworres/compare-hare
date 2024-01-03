#region usings

using CompareHare.Api.Features.Authentication.Models;
using CompareHare.Api.Features.Shared.Validation.Interfaces;

#endregion

namespace CompareHare.Api.Features.Authentication.RequestHandlers.LogIn
{
    public class LogInMessage : IValidatableRequest<LogInModel>
    {
        public LogInMessage(LogInModel model, string ip)
        {
            Model = model;
            Ip = ip;
        }

        public LogInModel Model { get; }
        public string Ip { get; }
    }
}
