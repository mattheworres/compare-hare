using Xunit;
using Autofac.Extras.Moq;
using CompareHare.Domain.Services.Interfaces;
using Shouldly;
using CompareHare.Domain.Services;
using CompareHare.Domain.Features.PriceScrapers;
using CompareHare.Domain.Entities.Constants;
using CompareHare.Tests.Domain.Services;
using AngleSharp.Io;
using System.IO;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace CompareHare.Tests.Domain.Features.Services
{
    public class DefaultPriceScraperTests
    {
        private const string DEFAULT_URL = "http://localhost:8000/public/Lowes_Response.html";
        private const string LOWES_SELECTOR = "span.aPrice span:nth-of-type(2)";
        private const string HD_SELECTOR = "div.price-format__large.price-format__main-price span:nth-of-type(2)";
        private const string BB_SELECTOR = "div.price-box div.priceView-hero-price.priceView-customer-price span:nth-of-type(1)";

        private async Task<float> TestMockedScraper(string relativePathToResponseFile, ProductRetailer retailer)
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                var mockedWrapper = autoMock.Mock<IParserWrapper>();
                // var mockEnvironment = autoMock.Mock<IHostingEnvironment>();
                // mockEnvironment
                //     .Setup(m => m.EnvironmentName)
                //     .Returns("Hosting:Development");
                var localDocumentFaker = new LocalDocumentFaker();
                var pathToFakeDoc = Path.Combine(Directory.GetCurrentDirectory(), relativePathToResponseFile);
                var fakeDocument = await localDocumentFaker.GetFakeDocument(pathToFakeDoc);
                mockedWrapper.Setup(x => x.OpenUrlAsync(It.IsAny<string>(), It.IsAny<IRequester>())).ReturnsAsync(fakeDocument);
                autoMock.Provide(mockedWrapper);
                // autoMock.Provide(mockEnvironment);
                var parserHelper = new ParserHelper();
                var productHelper = new ProductHelper();
                autoMock.Provide<IParserHelper>(parserHelper);
                autoMock.Provide<IProductHelper>(productHelper);

                var sut = autoMock.Create<DefaultPriceScraper>();
                var response = await sut.ScrapePrice(1, 1, retailer, DEFAULT_URL, null);

                return response.Price.HasValue ? response.Price.Value : 0;
            }
        }

        [Fact]
        public async void ItShouldScrapeLowesCorrectly()
        {
            var response = await TestMockedScraper("Domain/MockDocs/Products/Lowes_Response.html", ProductRetailer.Lowes);
            response.ShouldBe(2099f);
        }

        [Fact]
        public async void ItShouldScrapeHomeDepotCorrectly()
        {
            var response = await TestMockedScraper("Domain/MockDocs/Products/HD_Response.html", ProductRetailer.HomeDepot);
            response.ShouldBe(2098f);
        }

        [Fact]
        public async void ItShouldScrapeBestBuyCorrectly()
        {
            var response = await TestMockedScraper("Domain/MockDocs/Products/BB_Response.html", ProductRetailer.BestBuy);
            response.ShouldBe(2099.99f);
        }

        [Fact]
        public async void ItShouldScrapeAppliancesConnectionCorrectly()
        {
            var response = await TestMockedScraper("Domain/MockDocs/Products/AppliancesConnection_Response.html", ProductRetailer.AppliancesConnection);
            response.ShouldBe(2099.00f);
        }
    }
}
