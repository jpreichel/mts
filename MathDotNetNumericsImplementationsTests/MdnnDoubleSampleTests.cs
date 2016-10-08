using System.Diagnostics.CodeAnalysis;
using MathDotNetNumericsImplementations;
using NUnit.Framework;

namespace MathDotNetNumericsImplementationsTests
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class MdnnDoubleSampleTests
    {
        private const int Variables = 5;
        private static readonly double[] Values = { 0.1, 2.3, 4.5, 6.7, 8.9 };
        private const double NewValue = -1.1;

        [Test]
        public void CanGetVariableCount()
        {
            var storage = new MdnnDoubleSample(Variables);

            Assert.AreEqual(Variables, storage.Variables);
        }

        [Test]
        public void CanAccessElementsViaSquareBrackets()
        {
            var storage = new MdnnDoubleSample(Variables) { [0] = Variables };

            Assert.AreEqual(Variables, storage[0]);
        }

        [Test]
        public void CanConstructUsingArray()
        {
            var storage = new MdnnDoubleSample(Values);

            Assert.AreEqual(Variables, storage.Variables);
        }

        [Test]
        public void AddIncreasesSize()
        {
            var storage = new MdnnDoubleSample(Variables);

            storage.Add(0);

            Assert.AreEqual(Variables + 1, storage.Variables);
        }

        [Test]
        public void AddToIndex0ShiftsContents()
        {
            var storage = new MdnnDoubleSample(Values);

            storage.Add(0, NewValue);

            Assert.AreEqual(NewValue, storage[0]);
            Assert.AreEqual(Values[0], storage[1]);
        }

        [Test]
        public void AddToMiddleShiftsFollowingContents()
        {
            const int insertIndex = 3;
            var storage = new MdnnDoubleSample(Values);

            storage.Add(insertIndex, NewValue);

            Assert.AreEqual(NewValue, storage[insertIndex]);
            Assert.AreEqual(Values[insertIndex], storage[insertIndex + 1]);
        }

        [Test]
        public void RemoveDecreasesSize()
        {
            var storage = new MdnnDoubleSample(Variables);

            storage.Remove(0);

            Assert.AreEqual(Variables - 1, storage.Variables);
        }

        [Test]
        public void RemoveIndex0ShiftsAll()
        {
            var storage = new MdnnDoubleSample(Values);

            storage.Remove(0);

            Assert.AreEqual(Values[1], storage[0]);
        }

        [Test]
        public void RemoveFromMiddleShiftsContents()
        {
            const int removeIndex = 3;
            var storage = new MdnnDoubleSample(Values);

            storage.Remove(removeIndex);

            Assert.AreEqual(Values[removeIndex + 1], storage[removeIndex]);
        }

        [Test]
        public void RemoveFromEndPreservesColumns()
        {
            const int removeIndex = 4;
            var storage = new MdnnDoubleSample(Values);

            storage.Remove(removeIndex);

            Assert.AreEqual(Values[0], storage[0]);
            Assert.AreEqual(Values[removeIndex - 1], storage[removeIndex - 1]);
        }
    }
}
