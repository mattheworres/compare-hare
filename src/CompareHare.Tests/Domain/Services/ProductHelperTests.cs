using Xunit;
using CompareHare.Domain.Services;
using Autofac.Extras.Moq;
using Shouldly;
using CompareHare.Domain.Entities.Constants;

namespace CompareHare.Tests.Domain.Services
{
    public class ProductHelperTests
    {
        [Fact]
        public void ItShouldReturnARetailersSelector()
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                var sut = autoMock.Create<ProductHelper>();

                var result = sut.GetRetailerSelector(ProductRetailer.Lowes);

                result.ShouldBe("span.aPrice span:nth-of-type(2)");
            }
        }

        [Fact]
        public void ItShouldReturnAnEmptyString()
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                var sut = autoMock.Create<ProductHelper>();

                var result = sut.GetRetailerSelector(ProductRetailer.Alibaba);

                result.ShouldBeNull();
            }
        }
    }
}
