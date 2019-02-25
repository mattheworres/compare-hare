using Xunit;
using Autofac.Extras.Moq;
using System.IO;
using Moq;
using AngleSharp.Io;
using System.Threading;
using CompareHare.Domain.Features.OfferLoaders;
using CompareHare.Domain.Services.Interfaces;
using CompareHare.Domain.Entities;
using Shouldly;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using System.Threading.Tasks;
using CompareHare.Domain.Services;
using System.Collections.Generic;
using AngleSharp;

namespace CompareHare.Tests.Domain.Features.Services
{
    public class PAPowerOfferLoaderTests
    {
        [Fact]//TODO: Run in debug and see which of the 19 offers are borking things
        public async void ItShouldDoSomething() {
            using(var autoMock = AutoMock.GetLoose()) {
                var mockedWrapper = autoMock.Mock<IParserWrapper>();
                var mockedIndex = autoMock.Mock<StateUtilityIndex>().SetupProperty(x => x.LoaderDataIdentifier, "12345");
                var fakeDocument = await GetFakeDocument();
                var blert = fakeDocument.QuerySelector("div").Text();
                mockedWrapper.Setup(x => x.OpenUrlAsync(It.IsAny<string>(), It.IsAny<IRequester>())).ReturnsAsync(fakeDocument);
                autoMock.Provide(mockedWrapper);

                var sut = autoMock.Create<PAPowerOfferLoader>();

                var offers = await sut.LoadOffers(mockedIndex.Object);

                offers.Count.ShouldBe(45);
            }
        }

        // [Fact]
        // public void ItShouldCreateA40CharacterHash()
        // {
        //     var blarp = GenerateABlerp();

        //     using (var autoMock = AutoMock.GetLoose())
        //     {
        //         var sut = autoMock.Create<ObjectHasher>();

        //         var result = sut.HashObject(blarp);

        //         result.Length.ShouldBe(40);
        //     }
        // }

        // [Fact]
        // public void ItShouldGenerateTheSameValueTwice()
        // {
        //     var bloop = GenerateABlerp();

        //     using (var autoMock = AutoMock.GetLoose())
        //     {
        //         var sut = autoMock.Create<ObjectHasher>();

        //         var result1 = sut.HashObject(bloop);
        //         var result2 = sut.HashObject(bloop);
        //         var result3 = sut.HashObject(bloop);

        //         result1.ShouldBe(result2);
        //         result2.ShouldBe(result3);
        //     }
        // }

        // [Fact]
        // public void ItShouldCreateDifferentHashesForOneChangedTitleCharacter()
        // {
        //     var bloop = GenerateABlerp();
        //     var blerp = GenerateABlerp("mbinTitle");

        //     using (var autoMock = AutoMock.GetLoose())
        //     {
        //         var sut = autoMock.Create<ObjectHasher>();

        //         var result1 = sut.HashObject(bloop);
        //         var result2 = sut.HashObject(blerp);

        //         result1.ShouldNotBe(result2);
        //     }
        // }

        // [Fact]
        // public void ItShouldCreateDifferentHashesForOneChangedChildCharacter()
        // {
        //     var bloop = GenerateABlerp("mainTitle");
        //     var blerp = GenerateABlerp("mainTitle", "s3condaryTitle");

        //     using (var autoMock = AutoMock.GetLoose())
        //     {
        //         var sut = autoMock.Create<ObjectHasher>();

        //         var result1 = sut.HashObject(bloop);
        //         var result2 = sut.HashObject(blerp);

        //         result1.ShouldNotBe(result2);
        //     }
        // }

        // [Fact]
        // public void ItShouldCreateTheSameHash50Times()
        // {
        //     var bloop = GenerateABlerp("sameHash!");

        //     using (var autoMock = AutoMock.GetLoose())
        //     {
        //         var sut = autoMock.Create<ObjectHasher>();
        //         var results = new List<string>();
        //         var mainResult = sut.HashObject(bloop);

        //         //Sanity check, this should never change unless HerpBlerp is updated
        //         mainResult.ShouldBe("82283592447D30841F97AC385439218F56F1DF4A");

        //         for (var i = 0; i < 50; i++)
        //         {
        //             results.Add(sut.HashObject(bloop));
        //         }

        //         foreach (var result in results)
        //         {
        //             result.ShouldBe(mainResult);
        //         }
        //     }
        // }

        private async Task<IDocument> GetFakeDocument() {
            var parserWrapper = new ParserWrapper();
            var requesterMock = GetDefaultRequesterMock();
            return await parserWrapper.OpenUrlAsync("http://askjdkaj", requesterMock.Object);
        }

        private Mock<FakeRequester> GetDefaultRequesterMock() {
            var mockResponse = new Mock<IResponse>();
            mockResponse.Setup(x => x.Address).Returns(new Url("fakeaddress"));
            mockResponse.Setup(x => x.Headers).Returns(new Dictionary<string, string>());
            mockResponse.Setup(_ => _.Content).Returns(LoadFakeDocumentFromFile());
            var mockFakeRequester = new Mock<FakeRequester>();

            mockFakeRequester.Setup(_ => _.RequestAsync(It.IsAny<Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockResponse.Object);
            mockFakeRequester.Setup(x => x.SupportsProtocol(It.IsAny<string>())).Returns(true);
            return mockFakeRequester;
        }

        private MemoryStream LoadFakeDocumentFromFile() {
            using (FileStream fileStream = File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), "Domain/Features/OfferLoaders/PA_Response.html")))
            {
                MemoryStream memStream = new MemoryStream();
                memStream.SetLength(fileStream.Length);
                fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

                return memStream;
            }
        }
    }

    public class FakeRequester : IRequester
    {
        public event DomEventHandler Requesting;
        public event DomEventHandler Requested;

        public void AddEventListener(string type, DomEventHandler callback = null, bool capture = false)
        {
            throw new System.NotImplementedException();
        }

        public bool Dispatch(Event ev)
        {
            throw new System.NotImplementedException();
        }

        public void InvokeEventListener(Event ev)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveEventListener(string type, DomEventHandler callback = null, bool capture = false)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<IResponse> RequestAsync(Request request, CancellationToken cancel)
        {
            throw new System.NotImplementedException();
        }

        public virtual bool SupportsProtocol(string protocol)
        {
            throw new System.NotImplementedException();
        }
    }
}
