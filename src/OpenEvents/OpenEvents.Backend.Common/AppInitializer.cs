using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenEvents.Backend.Common
{
    public class AppInitializer 
    {
        
        public async Task RunInitializerTasks(IServiceProvider provider, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var types = assembly.FindAllImplementations<IAppInitializerTask>();

                foreach (var type in types)
                {
                    await RunTask(provider, type);
                }
            }
        }

        private async Task RunTask(IServiceProvider provider, Type type)
        {
            var instance = (IAppInitializerTask)provider.GetService(type);
            await instance.Initialize();
        }
    }
}
