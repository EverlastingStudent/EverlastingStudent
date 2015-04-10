namespace EverlastingStudent.Common.Infrastructure.Automapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using AutoMapper;

    public class AutoMapperConfig
    {
        private readonly IEnumerable<Assembly> assemblies;

        public AutoMapperConfig(IEnumerable<Assembly> assemblies)
        {
            this.assemblies = assemblies;
        }

        public void Execute()
        {
            var types = this.assemblies.SelectMany(a => a.GetExportedTypes()).ToList();

            LoadStandardMappings(types);

            LoadCustomMappings(types);
        }

        private static void LoadStandardMappings(IEnumerable<Type> types)
        {
            var maps = (from t in types
                        from i in t.GetInterfaces()
                        where
                            i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>) && !t.IsAbstract
                            && !t.IsInterface
                        select new { Source = i.GetGenericArguments()[0], Destination = t }).ToArray();

            foreach (var map in maps)
            {
                Mapper.CreateMap(map.Source, map.Destination);
                Mapper.CreateMap(map.Destination, map.Source);
            }
        }

        private static void LoadCustomMappings(IEnumerable<Type> types)
        {
            var maps = (from t in types
                        from i in t.GetInterfaces()
                        where typeof(IHaveCustomMappings).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface
                        select (IHaveCustomMappings)Activator.CreateInstance(t)).ToArray();

            foreach (var map in maps)
            {
                map.CreateMappings(Mapper.Configuration);
            }
        }
    }
}
