using System.Collections.Generic;
using System.Linq;
using MahalanobisTaguchiSystem.Interfaces;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace MathDotNetNumericsImplementations
{
    public class MdnnDoubleSample : ISample<double>
    {
        private Vector<double> _storage;
        public double[] Storage => _storage.ToArray();

        public double this[int index]
        {
            get { return _storage[index]; }
            set { _storage[index] = value; }
        }

        public MdnnDoubleSample(int length)
        {
            _storage = new DenseVector(length);
        }

        public MdnnDoubleSample(IEnumerable<double> values)
        {
            _storage = new DenseVector(values.ToArray());
        }

        public int Variables => _storage.Count;

        public void Add(double value = 0)
        {
            var oldStorage = _storage;
            _storage = new DenseVector(oldStorage.Count + 1);
            oldStorage.CopySubVectorTo(_storage, 0, 0, oldStorage.Count);
            _storage[oldStorage.Count] = value;
        }

        public void Add(int index, double value = 0)
        {
            var oldStorage = _storage;
            _storage = new DenseVector(oldStorage.Count + 1);
            if (index == 0)
            {
                oldStorage.CopySubVectorTo(_storage, 0, 1, oldStorage.Count);
                _storage[0] = value;
            }
            else
            {
                oldStorage.CopySubVectorTo(_storage, 0, 0, index - 1);
                _storage[index] = value;
                oldStorage.CopySubVectorTo(_storage, index, index + 1, oldStorage.Count - index);
            }
        }

        public void Remove(int index)
        {
            var oldStorage = _storage;
            _storage = new DenseVector(oldStorage.Count - 1);

            for (int newIndex = 0, oldIndex = 0; oldIndex < oldStorage.Count; ++oldIndex)
            {
                if (index != oldIndex)
                    _storage[newIndex++] = oldStorage[oldIndex];
            }
        }
    }
}