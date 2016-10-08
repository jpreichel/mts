using System.Diagnostics.CodeAnalysis;
using MathDotNetNumericsImplementations;
using NUnit.Framework;

namespace MathDotNetNumericsImplementationsTests
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class MdnnDoubleSpaceTests
    {
        private const int Samples = 2;
        private const int Variables = 3;
        private static readonly double[] Values = { 0.1, 2.3, 4.5, 6.7, 8.9, 10.1 };
        private const double NewValue = -1.0;
        private static readonly double[] NewSample = { -1.0, -2.3, -5.4 };
        private static readonly double[] NewVariable = { -7.6, -9.8 };

        [Test]
        public void CanGetVariableCount()
        {
            var storage = new MdnnDoubleSpace(Samples, Variables);

            Assert.AreEqual(Samples, storage.Samples);
            Assert.AreEqual(Variables, storage.Variables);
        }

        [Test]
        public void CanAccessElementsViaSquareBrackets()
        {
            var storage = new MdnnDoubleSpace(Samples, Variables, Values) { [0, 0] = NewValue };

            Assert.AreEqual(NewValue, storage[0, 0]);
        }

        [Test]
        public void CanRetrievePassedArray()
        {
            var valuesAsTwoD = new[,] { { Values[0], Values[1], Values[2] }, { Values[3], Values[4], Values[5] } };
            var storage = new MdnnDoubleSpace(Samples, Variables, Values);

            CollectionAssert.AreEqual(valuesAsTwoD, storage.Storage);
        }

        [Test]
        public void ConstructorsProduceEquivalentStorage()
        {
            var valuesAsTwoD = new[,] { { Values[0], Values[1], Values[2] }, { Values[3], Values[4], Values[5] } };

            var oneDimensionalArgument = new MdnnDoubleSpace(Samples, Variables, Values);
            var twoDimensionalArgument = new MdnnDoubleSpace(valuesAsTwoD);

            CollectionAssert.AreEqual(oneDimensionalArgument.Storage, twoDimensionalArgument.Storage);
        }

        [Test]
        public void AddSampleIncreasesSampleCount()
        {
            var storage = new MdnnDoubleSpace(Samples, Variables, Values);
            storage.AddSample(NewSample);

            Assert.AreEqual(Samples + 1, storage.Samples);
            Assert.AreEqual(NewSample[0], storage[Samples, 0]);
        }

        [Test]
        public void AddSampleAtIndex0ShiftsContents()
        {
            var storage = new MdnnDoubleSpace(Samples, Variables, Values);
            storage.AddSample(0, NewSample);

            Assert.AreEqual(Samples + 1, storage.Samples);
            Assert.AreEqual(NewSample[0], storage[0, 0]);
            Assert.AreEqual(Values[0], storage[1, 0]);
        }

        [Test]
        public void AddSampleInMiddleShiftsContents()
        {
            var storage = new MdnnDoubleSpace(Samples, Variables, Values);
            storage.AddSample(1, NewSample);

            Assert.AreEqual(Samples + 1, storage.Samples);
            Assert.AreEqual(Values[0], storage[0, 0]);
            Assert.AreEqual(NewSample[0], storage[1, 0]);
        }

        [Test]
        public void AddVariableAppendsVariable()
        {
            var storage = new MdnnDoubleSpace(Samples, Variables, Values);
            storage.AddVariable(NewVariable);

            Assert.AreEqual(Variables + 1, storage.Variables);
            Assert.AreEqual(NewVariable[0], storage[0, Variables]);
        }

        [Test]
        public void AddVariableAtIndex0ShiftsContents()
        {
            var storage = new MdnnDoubleSpace(Samples, Variables, Values);
            storage.AddVariable(0, NewVariable);

            Assert.AreEqual(Variables + 1, storage.Variables);
            Assert.AreEqual(NewVariable[0], storage[0, 0]);
            Assert.AreEqual(Values[0], storage[0, 1]);
        }

        [Test]
        public void AddVariableInMiddleShiftsContents()
        {
            var storage = new MdnnDoubleSpace(Samples, Variables, Values);
            storage.AddVariable(1, NewVariable);

            Assert.AreEqual(Variables + 1, storage.Variables);
            Assert.AreEqual(Values[0], storage[0, 0]);
            Assert.AreEqual(NewVariable[0], storage[0, 1]);
        }

        [Test]
        public void RemoveSampleIndex0ShiftsContents()
        {
            var storage = new MdnnDoubleSpace(Samples, Variables, Values);
            storage.RemoveSample(0);

            Assert.AreEqual(Samples - 1, storage.Samples);
            Assert.AreEqual(Values[3], storage[0, 0]);
        }

        [Test]
        public void RemoveLastSampleShiftsContents()
        {
            var storage = new MdnnDoubleSpace(Samples, Variables, Values);
            storage.RemoveSample(1);

            Assert.AreEqual(Samples - 1, storage.Samples);
            Assert.AreEqual(Values[0], storage[0, 0]);
        }

        [Test]
        public void RemoveVariableIndex0ShiftsContents()
        {
            var storage = new MdnnDoubleSpace(Samples, Variables, Values);
            storage.RemoveVariable(0);

            Assert.AreEqual(Variables - 1, storage.Variables);
            Assert.AreEqual(Values[1], storage[0, 0]);
        }

        [Test]
        public void RemoveLastVariableShiftsContents()
        {
            var storage = new MdnnDoubleSpace(Samples, Variables, Values);
            storage.RemoveVariable(2);

            Assert.AreEqual(Variables - 1, storage.Variables);
            Assert.AreEqual(Values[0], storage[0, 0]);
        }

        [Test]
        public void RemoveMiddleVariableShiftsContents()
        {
            var storage = new MdnnDoubleSpace(Samples, Variables, Values);
            storage.RemoveVariable(1);

            Assert.AreEqual(Variables - 1, storage.Variables);
            Assert.AreEqual(Values[2], storage[0, 1]);
        }
    }
}