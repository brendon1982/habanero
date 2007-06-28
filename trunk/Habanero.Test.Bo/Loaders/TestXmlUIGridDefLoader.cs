using Habanero.Bo.Loaders;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.Bo.Loaders
{
    /// <summary>
    /// Summary description for TestXmlUIGridDefLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlUIGridDefLoader
    {
        private XmlUIGridDefLoader loader;

        [SetUp]
        public void SetupTest()
        {
            loader = new XmlUIGridDefLoader();
        }

        [Test]
        public void TestLoadPropertyCollection()
        {
            UIGridDef def =
                loader.LoadUIGridDef(
                    @"
					<grid>
						<column heading=""testheading1"" property=""testpropname1""  />
						<column heading=""testheading2"" property=""testpropname2""  />
					</grid>");
            Assert.AreEqual(2, def.Count);
            Assert.AreEqual("testheading1", def[0].Heading);
            Assert.AreEqual("testheading2", def[1].Heading);
        }
    }
}