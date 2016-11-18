using System;
using MahalanobisTaguchiSystem;
using MahalanobisTaguchiSystem.Interfaces;
using MathDotNetNumericsImplementations;
using Ninject;

namespace MtsExampleApplication
{
    class Program
    {
        private static IKernel _kernel;

        static void Main(string[] args)
        {
            ConfigureKernel();

            var factory = _kernel.Get<IMtsFactory<double>>();
            var target = factory.CreateSpaceFromArray(Good);
            var samples = factory.CreateSpaceFromArray(Bad);

            var mts = _kernel.Get<Mts<double>>();

            var variableUseful = mts.FindUsefulVariables(target, samples);
            Console.WriteLine(string.Join(" ", variableUseful));

            for (var i = 0; i < samples.Samples; ++i)
            {
                var sample = samples.GetSample(i);
                var distance = mts.GetMahalanobisDistance(target, sample);

                Console.WriteLine($"Sample #{i + 1}: {distance}");
            }

            Console.ReadLine();
        }

        private static void ConfigureKernel()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IMtsFactory<double>>().To<MdnnDoubleMtsFactory>();
            _kernel.Bind<IMtsMathProvider<double>>().To<MdnnDoubleMtsMathProvider>();
            _kernel.Bind<Mts<double>>().ToSelf();
        }

        private static readonly double[,] Good = {
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

        private static readonly double[,] Bad = {
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
