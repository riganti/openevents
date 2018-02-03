using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenEvents.Backend.Common;
using OpenEvents.Backend.Common.Mappings;
using OpenEvents.Backend.Events.Facades;
using OpenEvents.Backend.Orders.Facades;

namespace OpenEvents.Tests
{
    [TestClass]
    public class Initializer
    {

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext testContext)
        {
            Mapper.Initialize(mapper =>
            {
                var assemblies = new[]
                {
                    typeof(EventsFacade).Assembly,
                    typeof(OrdersFacade).Assembly
                };

                // find all mappings and register them automatically
                var mappings = assemblies.SelectMany(a => a.FindAllImplementations<IMapping>());
                foreach (var mapping in mappings)
                {
                    var instance = (IMapping)Activator.CreateInstance(mapping);
                    instance.Configure(mapper);
                }
            });
            Mapper.AssertConfigurationIsValid();
        }

    }
}
