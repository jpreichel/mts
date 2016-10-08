using System;
using MahalanobisTaguchiSystem;
using MathDotNetNumericsImplementations;
using NUnit.Framework;

namespace MahalanobisTaguchiSystemTests
{
    [TestFixture]
    public class MtsTests
    {
        private MTS _mts;

        [SetUp]
        public void Setup()
        {
            var provider = new MdnnMtsMathProvider();
            var factory = new MdnnMtsDoubleFactory();
            _mts = new MTS(provider, factory);
        }

        [Test]
        public void CalculateZCorrectlyDeterminesZeroZ()
        {

            var matrix = new double[,] { { 0, 0, 0 }, { 0, 0, 0 } };
            var space = new MdnnDoubleSpace(matrix);

            var vector = new double[] { 0, 0, 0 };
            var sample = new MdnnDoubleSample(vector);

            var z = _mts.CalculateZ(space, sample);

            CollectionAssert.AreEqual(vector, z.Storage);
        }

        [TestCase(1)]
        [TestCase(64)]
        [TestCase(128)]
        [TestCase(1024)]
        public void IsPowerOfTwoCorrectlyIdentifiesPowerOfTwo(int n)
        {
            var result = MTS.IsPowerOfTwo(n);
            Assert.IsTrue(result);
        }

        [TestCase(0)]
        [TestCase(3)]
        [TestCase(17)]
        [TestCase(104)]
        public void IsPowerOfTwoCorrectlyIdentifiesNotPowerOfTwo(int n)
        {
            var result = MTS.IsPowerOfTwo(n);
            Assert.IsFalse(result);
        }

        [TestCase(0, 1)]
        [TestCase(3, 4)]
        [TestCase(32, 32)]
        [TestCase(104, 128)]
        [TestCase(663, 1024)]
        public void CeilingToPowerOfTwoCorrectlyCeilings(int n, int expected)
        {
            var result = MTS.CeilingToPowerOfTwo(n);
            Assert.AreEqual(expected, result);
        }

        [TestCase(0, 1)]
        [TestCase(3, 8)]
        [TestCase(5, 32)]
        [TestCase(7, 128)]
        [TestCase(10, 1024)]
        public void TwoToTheNthCorrectlyExponentiates(int n, int expected)
        {
            var result = MTS.GetTwoToTheNthPower(n);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetOrthogonalSpaceComputesCorrectSpaceForL16()
        {
            var expected = new double[,]
            {
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2},
                {1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 2, 2, 2, 2},
                {1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1},
                {1, 2, 2, 1, 1, 2, 2, 1, 1, 2, 2, 1, 1, 2, 2},
                {1, 2, 2, 1, 1, 2, 2, 2, 2, 1, 1, 2, 2, 1, 1},
                {1, 2, 2, 2, 2, 1, 1, 1, 1, 2, 2, 2, 2, 1, 1},
                {1, 2, 2, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 2, 2},
                {2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2},
                {2, 1, 2, 1, 2, 1, 2, 2, 1, 2, 1, 2, 1, 2, 1},
                {2, 1, 2, 2, 1, 2, 1, 1, 2, 1, 2, 2, 1, 2, 1},
                {2, 1, 2, 2, 1, 2, 1, 2, 1, 2, 1, 1, 2, 1, 2},
                {2, 2, 1, 1, 2, 2, 1, 1, 2, 2, 1, 1, 2, 2, 1},
                {2, 2, 1, 1, 2, 2, 1, 2, 1, 1, 2, 2, 1, 1, 2},
                {2, 2, 1, 2, 1, 1, 2, 1, 2, 2, 1, 2, 1, 1, 2},
                {2, 2, 1, 2, 1, 1, 2, 2, 1, 1, 2, 1, 2, 2, 1}
            };

            var result = _mts.GetOrthogonalSpace(14);

            CollectionAssert.AreEqual(expected, result.Storage);
        }

        [Test]
        public void GetOrthogonalSpaceComputesCorrectSpaceForL3()
        {
            var expected = new double[,]
            {
                {1, 1, 1},
                {1, 2, 2},
                {2, 1, 2},
                {2, 2, 1}
            };

            var result = _mts.GetOrthogonalSpace(3);

            CollectionAssert.AreEqual(expected, result.Storage);
        }

        [TestCase(new double[] { 10, 10, 8, 10, 6, 5, 10, 3, 1 }, 75.8)]
        [TestCase(new double[] { 5, 1, 1, 3, 4, 1, 3, 2, 1 }, 1.754)]
        [TestCase(new double[] { 10, 10, 10, 7, 9, 10, 7, 10, 10 }, 333)]
        [TestCase(new double[] { 8, 4, 4, 1, 2, 9, 3, 3, 1 }, 19.76)]
        public void GetMahalanobisDistanceComputesCorrectly(double[] sampleValues, double expectedMd)
        {
            var space = new MdnnDoubleSpace(GetGoodSpaceData());
            var sample = new MdnnDoubleSample(sampleValues);

            var result = _mts.GetMahalanobisDistance(space, sample);
            
            Assert.IsTrue(Math.Abs(result - expectedMd) < 0.1);
        }

        [Test]
        public void FindUsefulVariablesDeterminesCorrectUsefulVariables()
        {
            var expected = new[] {false, true, true, true, false, true, true, true, true};

            var goodSpace = new MdnnDoubleSpace(GetGoodSpaceData());
            var badSpace = new MdnnDoubleSpace(GetBadSpaceData());

            var result = _mts.FindUsefulVariables(goodSpace, badSpace);

            CollectionAssert.AreEqual(expected, result);
        }

        private static double[,] GetGoodSpaceData()
        {
            return new double[,]
            {
                {5, 1, 1, 3, 4, 1, 3, 2, 1},        
                {5, 1, 2, 10, 4, 5, 2, 1, 1},
                {1, 1, 1, 1, 2, 1, 2, 1, 1},
                {1, 1, 1, 1, 2, 5, 1, 1, 1},
                {5, 1, 1, 6, 3, 1, 1, 1, 1},
                {5, 1, 1, 1, 2, 1, 2, 2, 1},
                {3, 1, 1, 1, 2, 1, 3, 1, 1},
                {4, 1, 2, 1, 2, 1, 3, 1, 1},
                {5, 1, 1, 1, 2, 1, 1, 1, 1},
                {5, 1, 1, 1, 2, 2, 2, 1, 1},
                {4, 1, 3, 3, 2, 1, 1, 1, 1},
                {5, 2, 2, 2, 2, 1, 1, 1, 2},
                {3, 1, 1, 3, 2, 1, 1, 1, 1},
                {5, 1, 3, 1, 2, 1, 2, 1, 1},
                {5, 1, 1, 1, 2, 1, 2, 2, 1},
                {1, 1, 1, 2, 2, 1, 3, 1, 1},
                {1, 3, 1, 1, 2, 1, 2, 2, 1},
                {4, 2, 1, 1, 2, 2, 3, 1, 1},
                {5, 1, 1, 1, 2, 1, 1, 1, 1}
            };
        }

        private static double[,] GetBadSpaceData()
        {
            return new double[,]
            {
                {10, 10, 8, 10, 6, 5, 10, 3, 1},
                {10, 10, 10, 7, 9, 10, 7, 10, 10},
                {7, 9, 4, 10, 10, 3, 5, 3, 3},
                {5, 10, 10, 8, 5, 5, 7, 10, 1},
                {5, 5, 5, 2, 5, 10, 4, 3, 1},
                {8, 6, 5, 4, 3, 10, 6, 1, 1},
                {8, 4, 4, 1, 2, 9, 3, 3, 1},
                {4, 2, 3, 5, 3, 8, 7, 6, 1},
                {6, 1, 3, 1, 4, 5, 5, 10, 1},
                {10, 4, 7, 2, 2, 8, 6, 1, 1},
                {9, 5, 8, 1, 2, 3, 2, 1, 5}
            };
        }
    }
}
