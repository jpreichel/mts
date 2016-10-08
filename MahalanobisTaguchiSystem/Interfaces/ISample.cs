using System;

namespace MahalanobisTaguchiSystem.Interfaces
{
    // T : int, double, long, etc.
    public interface ISample<T> where T : struct,
          IComparable,
          IComparable<T>,
          IConvertible,
          IEquatable<T>,
          IFormattable
    {
        double this[int index] { get; set; }
        int Variables { get; }
        void Add(T value);
        void Add(int index, T value);
        void Remove(int index);
        T[] Storage { get; }
    }
}