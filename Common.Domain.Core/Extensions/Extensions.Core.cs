using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Core.Extensions
{
    public static partial class Extensions
    {
        public static T Tap<T>(this T @object, Action<T> action)
        {
            action(@object);
            return @object;
        }
    }
}
