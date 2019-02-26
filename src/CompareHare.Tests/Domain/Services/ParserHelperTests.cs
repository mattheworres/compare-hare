using Xunit;
using CompareHare.Domain.Services;
using Autofac.Extras.Moq;
using Shouldly;
using AngleSharp.Dom;
using Moq;
using System.Linq;
using System;

namespace CompareHare.Tests.Domain.Services
{
    public class ParserHelperTests
    {
        [Fact]
        public void ItShouldDetectAClassInAnElement()
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                var mockElement = autoMock.Mock<IElement>();
                mockElement.Setup(x => x.GetAttribute(It.IsAny<string>())).Returns("a very short string");

                var sut = autoMock.Create<ParserHelper>();

                var result = sut.ElementHasClass(mockElement.Object, "short");

                result.ShouldBeTrue();

                result = sut.ElementHasClass(mockElement.Object, "nonsense");

                result.ShouldBeFalse();

                result = sut.ElementHasClass(mockElement.Object, "sHoRt");

                result.ShouldBeFalse();
            }
        }

        [Fact]
        public void ItShouldDetectClassInsensitively() {
            using (var autoMock = AutoMock.GetLoose())
            {
                var mockElement = autoMock.Mock<IElement>();
                mockElement.Setup(x => x.GetAttribute(It.IsAny<string>())).Returns("a very short string");

                var sut = autoMock.Create<ParserHelper>();

                var result = sut.ElementHasClass(mockElement.Object, "sHoRt", false);

                result.ShouldBeTrue();
            }
        }

        [Fact]
        public void ItShouldReturnAnEnumerableOfClasses() {
            using (var autoMock = AutoMock.GetLoose())
            {
                var mockElement = autoMock.Mock<IElement>();
                mockElement.Setup(x => x.GetAttribute(It.IsAny<string>())).Returns("a very short string");

                var sut = autoMock.Create<ParserHelper>();

                var result = sut.GetClasses(mockElement.Object);

                result.ShouldNotBeNull();
                result.Count().ShouldBe(4);
                result.Any(x => x == "a").ShouldBeTrue();
                result.Any(x => x == "very").ShouldBeTrue();
                result.Any(x => x == "short").ShouldBeTrue();
                result.Any(x => x == "string").ShouldBeTrue();
            }
        }

        [Fact]
        public void ItShouldReturnTheClassByStartsWith() {
            using (var autoMock = AutoMock.GetLoose())
            {
                var mockElement = autoMock.Mock<IElement>();
                mockElement.Setup(x => x.GetAttribute(It.IsAny<string>())).Returns("a very short string");

                var sut = autoMock.Create<ParserHelper>();

                var result = sut.GetElementClassStartingWith(mockElement.Object, "ve");

                result.ShouldNotBeNullOrEmpty();
                result.ShouldBe("very");
            }
        }

        [Fact]
        public void ItShouldParseTheFirstFloatFromAString()
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                var sut = autoMock.Create<ParserHelper>();

                var result = sut.ParseFirstFloatFromString("With testing you now have 58.21% less time on your hands, thats 21.9 less!");

                result.ShouldBe(58.21f);

                Should.Throw<Exception>(() => sut.ParseFirstFloatFromString("I have no floats, sorry"));
            }
        }

        [Fact]
        public void ItShouldParseTheFirstIntegerFromTheString()
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                var sut = autoMock.Create<ParserHelper>();

                var result = sut.ParseFirstIntegerFromString("With testing you now have 58% less time on your hands, thats 21 less!");

                result.ShouldBe(58);

                Should.Throw<Exception>(() => sut.ParseFirstIntegerFromString("I have no ints, sorry"));
            }
        }
    }
}
