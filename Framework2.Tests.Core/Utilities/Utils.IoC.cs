using Framework2.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Framework2.Tests.Core.Utilities
{
    public static partial class Utils
    {
        public static IServiceProvider CreateServiceProvider(Action<ServiceCollection> configureServices)
        {
            var services = new ServiceCollection()
                .Tap(x => configureServices(x));
            return services.BuildServiceProvider();
        }
    }
}
