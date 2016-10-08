using MahalanobisTaguchiSystem.Interfaces;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;

namespace MathDotNetNumericsImplementations
{
    public class MdnnMtsMathProvider : IMtsMathProvider<double>
    {
        public ISample<double> Multiply(ISample<double> sample, ISpace<double> space)
        {
            var vector = DenseVector.OfArray(sample.Storage);
            var matrix = DenseMatrix.OfArray(space.Storage);
            var result = vector * matrix;

            return new MdnnDoubleSample(result.ToArray());
        }

        public ISpace<double> GetInverseSpace(ISpace<double> space)
        {
            var matrix = DenseMatrix.OfArray(space.Storage);
            var inverse = matrix.Inverse();

            return new MdnnDoubleSpace(inverse.Storage.ToArray());
        }

        public double GetCorrelation(double[] a, double[] b)
        {
            return Correlation.Pearson(a, b);
        }
    }
}
