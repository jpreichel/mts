using System.Diagnostics.CodeAnalysis;
using MathDotNetNumericsImplementations;
using NUnit.Framework;

namespace MathDotNetNumericsImplementationsTests
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class MdnnMtsMathProviderTests
    {
        [Test]
        public void MultiplyGetsCorrectResultForSpaceAndSample()
        {
            var values1 = new double[,]
            {
                {1, -1, 2},
                {0, -3, 1}
            };
            var space = new MdnnDoubleSpace(values1);

            var values2 = new double[] { 2, 1 };
            var sample = new MdnnDoubleSample(values2);

            var result = new MdnnMtsMathProvider().Multiply(sample, space);

            var expected = new double[] { 2, -5, 5 };

            CollectionAssert.AreEqual(expected, result.Storage);
        }
    }
}
