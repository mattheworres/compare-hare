using System.Threading.Tasks;
using AngleSharp.Dom;

namespace CompareHare.Tests.Domain.Services.Interfaces
{
    public interface ILocalDocumentFaker
    {
        Task<IDocument> GetFakeDocument(string pathToLocalDocument);
    }
}