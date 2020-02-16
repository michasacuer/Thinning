namespace Thinning.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Security.Policy;
    using Thinning.Algorithm.Interfaces;
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
            AppDomain currentDomain = AppDomain.CurrentDomain;
            Evidence asEvidence = currentDomain.Evidence;
            var ass = currentDomain.GetAssemblies();

            var algorithmAssembly = typeof(IAlgorithm).Assembly;
            var algorithmAssemblies = new List<TypeInfo>();
            foreach (var assembly in ass)
            {
                algorithmAssemblies.AddRange(assembly.DefinedTypes.Where(type =>
                    type.ImplementedInterfaces.Any(i => i == typeof(IAlgorithm)) && type.IsClass).ToList());
            }

            //var algorithmAssemblies = algorithmAssembly.DefinedTypes.Where(type =>
            //        type.ImplementedInterfaces.Any(i => i == typeof(IAlgorithm)) && type.IsClass).ToList();

            return algorithmAssemblies;
        }
    }
}
