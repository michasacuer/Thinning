namespace Thinning.Infrastructure
{
    using Microsoft.CSharp;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Thinning.Algorithm.Interfaces;
    using Thinning.Infrastructure.Interfaces;

    public class ApplicationSetup : IApplicationSetup
    {
        public List<string> GetRegisteredAlgorithmNames()
        {
            var interfacesNames = new List<string>();
            var assemblies = this.GetAlgorithmClasses();
            assemblies.ForEach(a => interfacesNames.Add(a.Name));

            return interfacesNames;
        }

        public List<IAlgorithm> GetRegisteredAlgorithmInstances()
        {
            var assemblies = this.GetAlgorithmClasses();
            var algorithmInstances = new List<IAlgorithm>();
            assemblies.ForEach(algorithm => algorithmInstances.Add((IAlgorithm)Activator.CreateInstance(algorithm)));

            return algorithmInstances;
        }

        public bool TryUploadCSharClassAlgorithm(string filepath)
        {
            var provider = new CSharpCodeProvider();
            var options = new CompilerParameters
            {
                OutputAssembly = $"{Guid.NewGuid().ToString()}.dll"
            };

            options.ReferencedAssemblies.Add(@"Thinning.Algorithm.dll");
            options.GenerateInMemory = true;
            string source = File.ReadAllText($@"{filepath}");

            var result = provider.CompileAssemblyFromSource(options, new[] { source });

            if(result.Errors.HasErrors)
            {
                return false;
            }

            return true;
        }

        private List<TypeInfo> GetAlgorithmClasses()
        {
            var currentDomain = AppDomain.CurrentDomain;
            var assemblies = currentDomain.GetAssemblies();

            var algorithmAssembly = typeof(IAlgorithm).Assembly;
            var algorithmClasses = new List<TypeInfo>();
            foreach (var assembly in assemblies)
            {
                algorithmClasses.AddRange(assembly.DefinedTypes.Where(type =>
                    type.ImplementedInterfaces.Any(i => i == typeof(IAlgorithm)) && type.IsClass).ToList());
            }

            return algorithmClasses;
        }
    }
}
