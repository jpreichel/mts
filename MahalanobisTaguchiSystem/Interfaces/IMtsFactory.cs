using System;

namespace MahalanobisTaguchiSystem.Interfaces
{
    // T : int, double, long, etc.
    public interface IMtsFactory<T> where T : struct,
        IComparable,
        IComparable<T>,
        IConvertible,
        IEquatable<T>,
        IFormattable
    {
        ISpace<T> CreateSpaceFromArray(T[,] array);
        ISpace<T> CreateSingleVariableSpaceFromSample(ISample<T> sample);
        ISpace<T> CreateSingleSampleSpaceFromSample(ISample<T> sample);
        ISample<T> CreateSampleFromArray(T[] array);
    }
}