namespace CompareHare.Api.Features.Shared.Services.Email.Interfaces
{
    public interface IEmailTemplateHelper
    {
        string ReadTemplate(string templateFileName);
        string CompileTemplate(string templateFileName, dynamic templateParameters);
    }
}
