using System.IO;
using System.Reflection;
using CompareHare.Api.Features.Shared.Services.Email.Interfaces;
using CompareHare.Domain.Features.Interfaces;
using HandlebarsDotNet;

namespace CompareHare.Api.Features.Shared.Services.Email
{
    public class EmailTemplateHelper : IEmailTemplateHelper, IFeatureService
    {
        /*
            Given the name of an email template file (*.hbs) stored within the
            CompareHare.Api.Emails DLL, return the contents of said template
         */
        public string ReadTemplate(string templateFileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string[] resources = assembly.GetManifestResourceNames();

            var stream = assembly.GetManifestResourceStream($"CompareHare.Api.Emails.{templateFileName}");

            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        /*
            Using Handlebars.Net, compile a template file (*.hbs) from the CompareHare.Api.Emails DLL
            and perform token replacement on the template with a set of passed parameters.
        */
        public string CompileTemplate(string templateFileName, dynamic templateParameters)
        {
            var templateHtml = ReadTemplate(templateFileName);
            var compiledTemplate = Handlebars.Compile(templateHtml);

            return compiledTemplate(templateParameters);
        }
    }
}
