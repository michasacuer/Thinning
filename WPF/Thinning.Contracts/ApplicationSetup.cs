namespace Thinning.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
    }
}
