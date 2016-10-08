using MahalanobisTaguchiSystem.Interfaces;
using MathNet.Numerics.LinearAlgebra.Double;

namespace MathDotNetNumericsImplementations
{
    public class MdnnMtsDoubleFactory : IMtsFactory<double>
    {
        public ISpace<double> CreateSpaceFromArray(double[,] array)
        {
            return new MdnnDoubleSpace(array);
        }

        public ISpace<double> CreateSingleVariableSpaceFromSample(ISample<double> sample)
        {
            var vector = DenseVector.OfArray(sample.Storage);
            var space = vector.ToColumnMatrix();

            return new MdnnDoubleSpace(space.ToArray());
        }

        public ISpace<double> CreateSingleSampleSpaceFromSample(ISample<double> sample)
        {
            var vector = DenseVector.OfArray(sample.Storage);
            var space = vector.ToRowMatrix();

            return new MdnnDoubleSpace(space.ToArray());
        }

        public ISample<double> CreateSampleFromArray(double[] array)
        {
            return new MdnnDoubleSample(array);
        }
    }
}