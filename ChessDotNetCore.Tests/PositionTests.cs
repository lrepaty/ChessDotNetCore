namespace ChessDotNetCore.Tests
{
    [TestClass]
    public class PositionTests
    {
        [TestMethod]
        public void TestEquality()
        {
            Position position1 = new Position(Line.c, 6);
            Position position2 = new Position(Line.c, 6);
            position1.Should().BeEquivalentTo(position1, "position1 and position2 are not equal");
            position1.Should().BeEquivalentTo(position2, "position2 and position1 are not equal");
            position2.Equals(position1).Should().BeTrue("position2.Equals(position1) should be true");
            (position1 == position2).Should().BeTrue("position1 == position2 should be true");
            (position2 == position1).Should().BeTrue("position2 == position1 should be true");
            (position1 != position2).Should().BeFalse("position1 != position2 should be false");
            (position2 != position1).Should().BeFalse("position2 != position1 should be false");
            position1.GetHashCode().Should().Be(position2.GetHashCode(), "Hash codes should be equal");
        }

        [TestMethod]
        public void TestInequality()
        {
            Position position1 = new Position(Line.e, 2);
            Position position2 = new Position(Line.b, 5);
            position1.Should().NotBeEquivalentTo(position2, "position1 and position2 are equal");
            position2.Should().NotBeEquivalentTo(position1, "position2 and position1 are equal");
            position2.Equals(position1).Should().BeFalse("position2.Equals(position1) should be false");
            (position1 == position2).Should().BeFalse("position1 == position2 should be false");
            (position2 == position1).Should().BeFalse("position2 == position1 should be false");
            (position1 != position2).Should().BeTrue("position1 != position2 should be true");
            (position2 != position1).Should().BeTrue("position2 != position1 should be true");
            position1.GetHashCode().Should().NotBe(position2.GetHashCode(), "Hash codes of position1 and position2 should be different");

            Position position3 = new Position(Line.e, 2);
            Position position4 = new Position(Line.e, 5);
            position3.Should().NotBeEquivalentTo(position4, "position1 and position2 are equal");
            position4.Should().NotBeEquivalentTo(position3, "position2 and position1 are equal");
            position3.Equals(position4).Should().BeFalse("position3.Equals(position4) should be false");
            (position3 == position4).Should().BeFalse("position3 == position4 should be false");
            (position4 == position3).Should().BeFalse("position4 == position3 should be false");
            (position3 != position4).Should().BeTrue("position3 != position4 should be true");
            (position4 != position3).Should().BeTrue("position4 != position3 should be true");
            position3.GetHashCode().Should().NotBe(position4.GetHashCode(), "Hash codes of position3 and position4 should be different");

            Position position5 = new Position(Line.e, 2);
            Position position6 = new Position(Line.b, 2);
            position5.Should().NotBeEquivalentTo(position6, "position5 and position6 are equal");
            position6.Should().NotBeEquivalentTo(position5, "position6 and position5 are equal");
            position5.Equals(position6).Should().BeFalse("position5.Equals(position6) should be false");
            (position5 == position6).Should().BeFalse("position5 == position6 should be false");
            (position6 == position5).Should().BeFalse("position6 == position5 should be false");
            (position5 != position6).Should().BeTrue("position5 != position6 should be true");
            (position6 != position5).Should().BeTrue("position6 != position5 should be true");
            position5.GetHashCode().Should().NotBe(position6.GetHashCode(), "Hash codes of position3 and position4 should be different");
        }

        [TestMethod]
        public void TestInequalityNull()
        {
            Position? position1 = new Position(Line.e, 1);
            Position? position2 = null;
            position2.Should().NotBeEquivalentTo(position1, "position1 and position2 should not be equal");
            position1.Equals(position2).Should().BeFalse("position1.Equals(position2) should be false");
            (position1 == position2).Should().BeFalse("position1 == position2 should be false");
            (position1 == position2).Should().BeFalse("position2 == position1 should be false");
            (position1 != position2).Should().BeTrue("position1 != position2 should be true");
            (position1 != position2).Should().BeTrue("position2 != position1 should be true");
        }

        [TestMethod]
        public void TestInequalityDifferentType()
        {
            Position position1 = new Position(Line.d, 3);
            string str = "abc";
            position1.Equals(str).Should().BeFalse("position1.Equals(str) should be false");
        }

        [TestMethod]
        public void TestConstructors()
        {
            new Position("A1").Should().BeEquivalentTo(new Position(Line.a, 1));
            new Position("B2").Should().BeEquivalentTo(new Position(Line.b, 2));
            new Position("C3").Should().BeEquivalentTo(new Position(Line.c, 3));
            new Position("D4").Should().BeEquivalentTo(new Position(Line.d, 4));
            new Position("E5").Should().BeEquivalentTo(new Position(Line.e, 5));
            new Position("F6").Should().BeEquivalentTo(new Position(Line.f, 6));
            new Position("G7").Should().BeEquivalentTo(new Position(Line.g, 7));
            new Position("H8").Should().BeEquivalentTo(new Position(Line.h, 8));
        }

        [TestMethod]
        public void TestToString()
        {
            new Position(Line.h, 5).ToString().Should().BeEquivalentTo("h5");
            new Position("h5").ToString().Should().BeEquivalentTo("h5");
        }
    }
}
