using System.Collections.Generic;
using System.Linq;
using MahalanobisTaguchiSystem.Interfaces;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace MathDotNetNumericsImplementations
{
    public class MdnnDoubleSpace : ISpace<double>
    {
        private Matrix<double> _storage;
        public double[,] Storage => _storage.Storage.ToArray();

        public MdnnDoubleSpace(int samples, int variables)
        {
            _storage = new DenseMatrix(samples, variables);
        }

        public MdnnDoubleSpace(int samples, int variables, double[] values)
        {
            var array = new double[samples, variables];
            for (int i = 0, k = 0; i < samples; i++)
            {
                for (var j = 0; j < variables; j++, k++)
                {
                    array[i, j] = values[k];
                }
            }

            _storage = DenseMatrix.OfArray(array);
        }

        public MdnnDoubleSpace(double[,] values)
        {
            _storage = DenseMatrix.OfArray(values);
        }

        public double this[int row, int column]
        {
            get { return _storage[row, column]; }
            set { _storage[row, column] = value; }
        }

        public int Samples => _storage.RowCount;
        public int Variables => _storage.ColumnCount;

        public void AddSample(IEnumerable<double> values = null)
        {
            var oldStorage = _storage;
            _storage = new DenseMatrix(oldStorage.RowCount + 1, oldStorage.ColumnCount);
            _storage.SetSubMatrix(0, 0, oldStorage);

            if (values != null)
                _storage.SetRow(oldStorage.RowCount, values.ToArray());
        }

        public void AddSample(int index, IEnumerable<double> values = null)
        {
            var oldStorage = _storage;
            _storage = new DenseMatrix(oldStorage.RowCount + 1, oldStorage.ColumnCount);

            var afterIndex = 0;
            for (var i = 0; i < oldStorage.RowCount; ++i)
            {
                if (i == index)
                    ++afterIndex;
                _storage.SetRow(i + afterIndex, oldStorage.Row(i));
            }
            if (values != null)
                _storage.SetRow(index, values.ToArray());
        }

        public void AddVariable(IEnumerable<double> values = null)
        {
            var oldStorage = _storage;
            _storage = new DenseMatrix(oldStorage.RowCount, oldStorage.ColumnCount + 1);
            _storage.SetSubMatrix(0, 0, oldStorage);

            if (values != null)
                _storage.SetColumn(oldStorage.ColumnCount, values.ToArray());
        }

        public void AddVariable(int index, IEnumerable<double> values = null)
        {
            var oldStorage = _storage;
            _storage = new DenseMatrix(oldStorage.RowCount, oldStorage.ColumnCount + 1);

            var afterIndex = 0;
            for (var i = 0; i < oldStorage.ColumnCount; ++i)
            {
                if (i == index)
                    ++afterIndex;
                _storage.SetColumn(i + afterIndex, oldStorage.Column(i));
            }

            if (values != null)
                _storage.SetColumn(index, values.ToArray());
        }

        public void RemoveSample(int index)
        {
            var oldStorage = _storage;
            _storage = new DenseMatrix(oldStorage.RowCount - 1, oldStorage.ColumnCount);

            for (int newIndex = 0, oldIndex = 0; oldIndex < oldStorage.RowCount; ++oldIndex)
            {
                if (index != oldIndex)
                {
                    _storage.SetRow(newIndex++, oldStorage.Row(oldIndex));
                }
            }
        }

        public void RemoveVariable(int index)
        {
            var oldStorage = _storage;
            _storage = new DenseMatrix(oldStorage.RowCount, oldStorage.ColumnCount - 1);

            for (int newIndex = 0, oldIndex = 0; oldIndex < oldStorage.ColumnCount; ++oldIndex)
            {
                if (index != oldIndex)
                {
                    _storage.SetColumn(newIndex++, oldStorage.Column(oldIndex));
                }
            }
        }

        public ISample<double> GetSample(int index)
        {
            var values = _storage.Row(index).ToArray();
            return new MdnnDoubleSample(values);
        }

        public double[] GetVariableValues(int index)
        {
            return _storage.Column(index).ToArray();
        }
    }
}