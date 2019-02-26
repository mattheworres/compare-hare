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
using System.Linq;
using System;

namespace CompareHare.Tests.Domain.Features.Services
{
    public class PAPowerOfferLoaderTests
    {
        private const string VARIABLE_PRICE_STRUCTURE = "Variable";
        private const string FIXED_PRICE_STRUCTURE = "Fixed";

        [Fact]
        public async void ItShouldParseBooleansCorrectly() {
            using(var autoMock = AutoMock.GetLoose()) {
                var mockedWrapper = autoMock.Mock<IParserWrapper>();
                var parserHelper = new ParserHelper();
                var mockedIndex = autoMock.Mock<StateUtilityIndex>().SetupProperty(x => x.LoaderDataIdentifier, "12345");
                var fakeDocument = await GetFakeDocument();
                mockedWrapper.Setup(x => x.OpenUrlAsync(It.IsAny<string>(), It.IsAny<IRequester>())).ReturnsAsync(fakeDocument);
                autoMock.Provide(mockedWrapper);
                autoMock.Provide(parserHelper);

                var sut = autoMock.Create<PAPowerOfferLoader>();

                var offers = await sut.LoadOffers(mockedIndex.Object);

                var numberOfBulkDiscounts = offers.Count(x => x.HasBulkDiscounts);
                var numberOfIntroductoryPrices = offers.Count(x => x.IsIntroductoryPrice);
                var numberOfRenewables = offers.Count(x => x.HasRenewable);
                var numberOfCancellationFees = offers.Count(x => x.HasCancellationFee);
                var numberOfEnrollmentFees = offers.Count(x => x.HasEnrollmentFee);
                var numberOfUniqueIdentifiers = offers.Select(x => x.OfferId).Distinct().Count();

                offers.Count.ShouldBe(95);

                numberOfBulkDiscounts.ShouldBe(10);
                numberOfIntroductoryPrices.ShouldBe(9);
                numberOfRenewables.ShouldBe(35);
                numberOfCancellationFees.ShouldBe(58);
                numberOfEnrollmentFees.ShouldBe(1);
                numberOfUniqueIdentifiers.ShouldBe(95);
            }
        }

        [Fact]
        public async void ItShouldParsePricesCorrectly()
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                var mockedWrapper = autoMock.Mock<IParserWrapper>();
                var parserHelper = new ParserHelper();
                var mockedIndex = autoMock.Mock<StateUtilityIndex>().SetupProperty(x => x.LoaderDataIdentifier, "12345");
                var fakeDocument = await GetFakeDocument();
                mockedWrapper.Setup(x => x.OpenUrlAsync(It.IsAny<string>(), It.IsAny<IRequester>())).ReturnsAsync(fakeDocument);
                autoMock.Provide(mockedWrapper);
                autoMock.Provide(parserHelper);

                var sut = autoMock.Create<PAPowerOfferLoader>();

                var offers = await sut.LoadOffers(mockedIndex.Object);

                var variableAmbitDeal = offers.FirstOrDefault(x => x.Name == "Ambit Energy" && x.PriceStructure == VARIABLE_PRICE_STRUCTURE);

                variableAmbitDeal.ShouldNotBeNull();
                variableAmbitDeal.PricePerUnit.HasValue.ShouldBeTrue();
                variableAmbitDeal.PricePerUnit.Value.ShouldBe(0.0960f);
                variableAmbitDeal.PriceUnit.ShouldBe("per kWh");
                variableAmbitDeal.HasRenewable.ShouldBeTrue();
                variableAmbitDeal.RenewablePercentage.HasValue.ShouldBeTrue();
                variableAmbitDeal.RenewablePercentage.Value.ShouldBe(100f);
                variableAmbitDeal.HasCancellationFee.ShouldBeFalse();
                variableAmbitDeal.HasBulkDiscounts.ShouldBeFalse();

                var blueRockDeal = offers.FirstOrDefault(x => x.Name == "BlueRock Energy Inc." && x.PriceStructure == FIXED_PRICE_STRUCTURE);

                blueRockDeal.ShouldNotBeNull();
                blueRockDeal.PricePerUnit.HasValue.ShouldBeTrue();
                blueRockDeal.PricePerUnit.Value.ShouldBe(0.1500f);
                blueRockDeal.PriceUnit.ShouldBe("per kWh");
                blueRockDeal.HasRenewable.ShouldBeFalse();
                blueRockDeal.HasCancellationFee.ShouldBeTrue();
                blueRockDeal.CancellationFee.ShouldBe("100.00");
                blueRockDeal.TermMonthLength.ShouldBe(12);

                var fesDeal = offers.FirstOrDefault(x => x.Name == "FirstEnergy Solutions" && x.HasRenewable);

                fesDeal.ShouldNotBeNull();
                fesDeal.HasRenewable.ShouldBeTrue();
                fesDeal.RenewablePercentage.HasValue.ShouldBeTrue();
                fesDeal.RenewablePercentage.Value.ShouldBe(100f);
                fesDeal.HasCancellationFee.ShouldBeTrue();
                fesDeal.CancellationFee.ShouldBe("50");
                fesDeal.TermEndDate.HasValue.ShouldBeTrue();
                var termEndDate = fesDeal.TermEndDate.HasValue ? fesDeal.TermEndDate.Value : new DateTime();
                termEndDate.Month.ShouldBe(1);
                termEndDate.Day.ShouldBe(31);
                termEndDate.Year.ShouldBe(2021);

                var agwayDeal = offers.FirstOrDefault(x => x.Name == "Agway Energy Services LLC" && x.HasRenewable);

                agwayDeal.ShouldNotBeNull();
                agwayDeal.SupplierPhone.ShouldBe("888-982-4929");
                agwayDeal.PricePerUnit.ShouldBe(0.069f);
                agwayDeal.IsIntroductoryPrice.ShouldBeTrue();
                agwayDeal.PriceStructure.ShouldBe(VARIABLE_PRICE_STRUCTURE);
                agwayDeal.RenewablePercentage.HasValue.ShouldBeTrue();
                agwayDeal.RenewablePercentage.Value.ShouldBe(100f);
                agwayDeal.HasTermEndDate.ShouldBeFalse();
            }
        }

        //Test Helpers
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
