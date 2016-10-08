using System;
using System.Collections.Generic;

namespace MahalanobisTaguchiSystem.Interfaces
{
    // T : int, double, long, etc.
    public interface ISpace<T> where T : struct,
          IComparable,
          IComparable<T>,
          IConvertible,
          IEquatable<T>,
          IFormattable
    {
        double this[int sample, int variable] { get; set; }
        int Samples { get; }
        int Variables { get; }
        void AddSample(IEnumerable<T> values);
        void AddSample(int index, IEnumerable<T> value);
        void AddVariable(IEnumerable<T> values);
        void AddVariable(int index, IEnumerable<T> value);
        void RemoveSample(int index);
        void RemoveVariable(int index);
        T[,] Storage { get; }
        ISample<T> GetSample(int index);
        T[] GetVariableValues(int index);
    }
}