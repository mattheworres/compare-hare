using System.IO;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Io;
using CompareHare.Tests.Domain.Services.Interfaces;
using Moq;
using System.Threading;
using CompareHare.Domain.Services;
using System.Collections.Generic;
using AngleSharp;

namespace CompareHare.Tests.Domain.Services
{
    public class LocalDocumentFaker : ILocalDocumentFaker
    {
        // Path: Path.Combine(Directory.GetCurrentDirectory(), "Domain/Features/OfferLoaders/PA_Response.html")
        public async Task<IDocument> GetFakeDocument(string pathToLocalDocument)
        {
            var parserWrapper = new ParserWrapper();
            var requesterMock = GetDefaultRequesterMock(pathToLocalDocument);
            return await parserWrapper.OpenUrlAsync("http://askjdjakj", requesterMock.Object);
        }

        private Mock<FakeRequester> GetDefaultRequesterMock(string pathToLocalDocument)
        {
            var mockResponse = new Mock<IResponse>();
            mockResponse.Setup(x => x.Address).Returns(new Url("fakeaddress"));
            mockResponse.Setup(x => x.Headers).Returns(new Dictionary<string, string>());
            mockResponse.Setup(_ => _.Content).Returns(LoadFakeDocumentFromFile(pathToLocalDocument));
            var mockFakeRequester = new Mock<FakeRequester>();

            mockFakeRequester.Setup(_ => _.RequestAsync(It.IsAny<Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockResponse.Object);
            mockFakeRequester.Setup(x => x.SupportsProtocol(It.IsAny<string>())).Returns(true);
            return mockFakeRequester;
        }

        private MemoryStream LoadFakeDocumentFromFile(string pathToLocalDocument)
        {
            using (FileStream fileStream = File.OpenRead(pathToLocalDocument))
            {
                MemoryStream memStream = new MemoryStream();
                memStream.SetLength(fileStream.Length);
                fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

                return memStream;
            }
        }
    }
}