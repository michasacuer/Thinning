namespace Thinning.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Thinning.Algorithm;
    using Thinning.Infrastructure.Interfaces;

    public class ApplicationSetup : IApplicationSetup
    {
        public List<string> GetRegisteredAlgorithmNames()
        {
            var interfacesNames = new List<string>();
            var assemblies = this.GetAlgorithmsAssemblies();
            assemblies.ForEach(a => interfacesNames.Add(a.Name));

            return interfacesNames;
        }

        public List<IAlgorithm> GetRegisteredAlgorithmInstances()
        {
            var assemblies = this.GetAlgorithmsAssemblies();
            var algorithmInstances = new List<IAlgorithm>();
            assemblies.ForEach(algorithm => algorithmInstances.Add((IAlgorithm)Activator.CreateInstance(algorithm)));

            return algorithmInstances;
        }

        private List<TypeInfo> GetAlgorithmsAssemblies()
        {
            var algorithmAssemly = typeof(K3M).Assembly;
            var algorithmAssemblies = algorithmAssemly.DefinedTypes.Where(type =>
                    type.ImplementedInterfaces.Any(inter => inter == typeof(IAlgorithm))).ToList();

            return algorithmAssemblies;
        }
    }
}
