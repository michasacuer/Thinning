namespace Thinning.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Thinning.Algorithm;
    using Thinning.Infrastructure.Interfaces;

    public class ApplicationSetup : IApplicationSetup
    {
        public List<string> GetRegisteredAlgorithmNames()
        {
            var interfaceType = typeof(IAlgorithm);
            var interfaces = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(interfaceType.IsAssignableFrom).ToList();

            interfaces.Remove(typeof(IAlgorithm));

            var interfacesNames = new List<string>();
            foreach (var type in interfaces)
            {
                interfacesNames.Add(type.Name);
            }

            return interfacesNames;
        }

        public List<IAlgorithm> GetRegisteredAlgorithmInstances()
        {
            var algorithmAssemly = typeof(K3M).Assembly;
            var algorithmAssemblies = algorithmAssemly.DefinedTypes.Where(type => type.ImplementedInterfaces.Any(inter => inter == typeof(IAlgorithm))).ToList();

            var algorithmInstances = new List<IAlgorithm>();
            foreach (var algorithm in algorithmAssemblies)
            {
                algorithmInstances.Add((IAlgorithm)Activator.CreateInstance(algorithm));
            }

            return algorithmInstances;
        }
    }
}
