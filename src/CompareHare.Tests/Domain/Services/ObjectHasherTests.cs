using System.Collections.Generic;
using Xunit;
using CompareHare.Domain.Services;
using Autofac.Extras.Moq;
using Shouldly;

namespace CompareHare.Tests.Domain.Services
{
    public class ObjectHasherTests
    {
        [Fact]
        public void ItShouldCreateA40CharacterHash()
        {
            var blarp = GenerateABlerp();

            using (var autoMock = AutoMock.GetLoose())
            {
                var sut = autoMock.Create<ObjectHasher>();

                var result = sut.HashObject(blarp);

                result.Length.ShouldBe(40);
            }
        }

        [Fact]
        public void ItShouldGenerateTheSameValueTwice()
        {
            var bloop = GenerateABlerp();

            using (var autoMock = AutoMock.GetLoose())
            {
                var sut = autoMock.Create<ObjectHasher>();

                var result1 = sut.HashObject(bloop);
                var result2 = sut.HashObject(bloop);
                var result3 = sut.HashObject(bloop);

                result1.ShouldBe(result2);
                result2.ShouldBe(result3);
            }
        }

        [Fact]
        public void ItShouldCreateDifferentHashesForOneChangedTitleCharacter()
        {
            var bloop = GenerateABlerp();
            var blerp = GenerateABlerp("mbinTitle");

            using (var autoMock = AutoMock.GetLoose())
            {
                var sut = autoMock.Create<ObjectHasher>();

                var result1 = sut.HashObject(bloop);
                var result2 = sut.HashObject(blerp);

                result1.ShouldNotBe(result2);
            }
        }

        [Fact]
        public void ItShouldCreateDifferentHashesForOneChangedChildCharacter()
        {
            var bloop = GenerateABlerp("mainTitle");
            var blerp = GenerateABlerp("mainTitle", "s3condaryTitle");

            using (var autoMock = AutoMock.GetLoose())
            {
                var sut = autoMock.Create<ObjectHasher>();

                var result1 = sut.HashObject(bloop);
                var result2 = sut.HashObject(blerp);

                result1.ShouldNotBe(result2);
            }
        }

        [Fact]
        public void ItShouldCreateTheSameHash50Times()
        {
            var bloop = GenerateABlerp("sameHash!");

            using (var autoMock = AutoMock.GetLoose())
            {
                var sut = autoMock.Create<ObjectHasher>();
                var results = new List<string>();
                var mainResult = sut.HashObject(bloop);

                //Sanity check, this should never change unless HerpBlerp is updated
                mainResult.ShouldBe("82283592447D30841F97AC385439218F56F1DF4A");

                for(var i = 0; i < 50; i++) {
                    results.Add(sut.HashObject(bloop));
                }

                foreach(var result in results) {
                    result.ShouldBe(mainResult);
                }
            }
        }

        //Test helper to generate a consistently complex object with some easy hooks to change it slightly
        private HerpBlerp GenerateABlerp(string mainTitle = "mainTitle", string secondaryTitle = "secondaryTitle")
        {
            var obj1 = new HerpBlerp(secondaryTitle, "Haska");
            var obj2 = new HerpBlerp("jskjask", "hdfjksdhsu");
            var list = new List<HerpBlerp> { obj1, obj2 };
            var obj3 = new HerpBlerp("hal", "Haska", list);
            var obj4 = new HerpBlerp("jskjask", "hdfjksdhsu", list);
            var list2 = new List<HerpBlerp> { obj3, obj4 };
            var obj5 = new HerpBlerp("hul", "Haska", list2);
            var obj6 = new HerpBlerp("jskjask", "hdfjksdhsu", list2);
            var list3 = new List<HerpBlerp> { obj5, obj6 };
            return new HerpBlerp(mainTitle, "ah yes, this is it", list3);
        }
    }

    //Nonsense class used to demonstrate that a non-flat object with C# structures and changes within them are property reflected
    public class HerpBlerp
    {
        public HerpBlerp(string herp, string blerp, List<HerpBlerp> herpBlerps = null)
        {
            Herp = herp;
            Derp = blerp;

            Blerps = herpBlerps;
        }

        public string Herp { get; set; }
        public string Derp { get; set; }

        public List<HerpBlerp> Blerps { get; set; }
    }
}
