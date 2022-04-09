using Common.Domain.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Tests.Utilities
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
